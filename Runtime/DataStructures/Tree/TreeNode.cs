using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCS.Core
{
    /// <summary>
    /// Represents the node of a basic tree. 
    /// - If you have one node, you have tree
    /// - Must have parent, unless root, then null
    /// - Contains children, can repeat value, can be any order 
    /// - Leaf contains no children
    /// - No caching, anything that can be calculated is
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T> : ATreeNodeBase<T> where T : IEquatable<T>
    {
        #region API
        /// <inheritdoc />
        public override ITreeNode<T> Root => IsRoot ? this : Parent.Root;

        /// <inheritdoc />
        public override T Value { get; protected set; }

        /// <inheritdoc />
        public override bool IsRoot => Parent == null;

        /// <inheritdoc />
        public override bool IsLeaf => _children.Count == 0;

        /// <inheritdoc />
        public override int DescendentCount => Children.Aggregate(0, (a, n) => a + n.DescendentCount, a => a);

        /// <inheritdoc />
        public override int ChildCount => _children.Count;

        /// <inheritdoc />
        public override int Depth => IsRoot ? 0 : 1 + Parent.Depth;

        /// <summary>
        /// Creates a new root tree node
        /// </summary>
        /// <param name="value">value of node</param>
        public TreeNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Node with default value
        /// </summary>
        public TreeNode()
        {
            Value = default(T);
        }

        /// <inheritdoc />
        public override ITreeNode<T> AddChild(T value)
        {
            TreeNode<T> node = new TreeNode<T>(value);
            InternalAddChild(this, node);
            return node;
        }

        /// <inheritdoc />
        public override void AddChildren(params T[] values)
        {
            foreach (T value in values)
            {
                AddChild(value);
            }
        }

        /// <inheritdoc />
        public override ITreeNode<T> GetChild(int index)
        {
            if (index >= ChildCount || index < 0)
            {
                throw new ArgumentOutOfRangeException($"No child at index {index}, node only has {ChildCount} children");
            }

            return _children[index];
        }

        /// <inheritdoc />
        public override void RemoveChild(int index)
        {
            if (index >= ChildCount || index < 0)
            {
                throw new ArgumentOutOfRangeException($"No child at index {index}, node only has {ChildCount} children");
            }

            _children.RemoveAt(index);
        }

        /// <inheritdoc />
        public override void RemoveChild(ITreeNode<T> node)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (node.Value.Equals(_children[i].Value))
                {
                    _children.RemoveAt(i);
                }
            }
        }

        /// <inheritdoc />
        public override bool SetValue(T value)
        {
            Value = value;
            return true;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Copy(ITreeNode<T> newParent = null)
        {
            // Either make new tree, or make a node with new parent as parent
            ITreeNode<T> newNode = newParent == null ?
                new TreeNode<T>(Value) :
                newParent.AddChild(Value);

            foreach (ITreeNode<T> child in _children)
            {
                child.Copy(newNode);
            }

            return newNode;
        }

        /// <inheritdoc />
        public override IEnumerable<ITreeNode<T>> SearchEnumeration(ETreeSearchType type)
        {
            switch (type)
            {
                case ETreeSearchType.BREADTHFIRST:
                    return new BasicEnumerable<TreeNode<T>>(
                        new BreathFirstSearchEnumerator<T>(this)
                    );
                case ETreeSearchType.DEPTHFIRST:
                    return new BasicEnumerable<TreeNode<T>>(
                        new DepthFirstSearchEnumerator<T>(this)
                    );
                default:
                    return null;
            }
        }

        /// <inheritdoc />
        public override ITreeNode<T> Union(ITreeNode<T> other)
        {
            return Union(other, UTITreeNode.BasicValueEquality<T>());
        }

        /// <inheritdoc />
        public override ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality)
        {
            return Union(other, equality, (o1, o2) => o1);
        }

        /// <inheritdoc />
        public override ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality, DMerge<T> merge)
        {
            // if there not equal, Union results in null
            if (!equality(this, other))
            {
                return null;
            }

            // Create a new node using the provided merge rule.
            // The merge rule determines how equal values resolve
            TreeNode<T> unionTree = new TreeNode<T>(merge(Value, other.Value));

            // Get the children of the other node, to test against the children of the current node
            List<ITreeNode<T>> testNodes = new List<ITreeNode<T>>(other.Children);

            // Foreach node in Children, preform a test to determine the case and handle
            // the union calculation
            foreach (ITreeNode<T> child in Children)
            {
                // candidates for a deeper union are testsNodes that equal the child
                IEnumerable<ITreeNode<T>> candidates = testNodes.Where(n => equality(n, child));

                // if no candidates, add this child to the new tree
                if (candidates == null || candidates.Count() == 0)
                {
                    child.Copy(unionTree);
                    continue;
                }

                // Determine which candidate should be used to perform the union
                // This is the most similar candidate
                ITreeNode<T> chosenCandidate = null;

                if (candidates.Count() == 1)
                {
                    chosenCandidate = candidates.First();
                }
                else
                {
                    chosenCandidate = child.MostSimilar(candidates);
                }

                // Perform a union with the chosen candidate
                InternalAddChild(unionTree, child.Union(chosenCandidate, equality, merge) as TreeNode<T>);

                // Remove the candidate
                testNodes.Remove(chosenCandidate);
            }

            // Add all remaining nodes without performing a merge
            foreach (ITreeNode<T> node in testNodes)
            {
                node.Copy(unionTree);
            }

            return unionTree;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Intersection(ITreeNode<T> other)
        {
            return Intersection(other, UTITreeNode.BasicValueEquality<T>());
        }

        /// <inheritdoc />
        public override ITreeNode<T> Intersection(ITreeNode<T> other, DEquality<ITreeNode<T>> equality)
        {
            // return null if other is null or if this does not equal other
            if (other == null || !equality(this, other))
            {
                return null;
            }

            // make a new node as the intersection tree
            TreeNode<T> intersectionTree = new TreeNode<T>(Value);

            // Get other children as nodes to test for intersection
            List<ITreeNode<T>> testNodes = new List<ITreeNode<T>>(other.Children);

            // Determine which candidate should be used to perform the intersection
            // This is the most similar candidate
            foreach (ITreeNode<T> child in Children)
            {
                // candidates are children that are equal
                IEnumerable<ITreeNode<T>> candidates = testNodes.Where(n => equality(n, child));

                // if there are no candidates, ignore this child. These are not included in the 
                // intersection
                if (candidates == null || candidates.Count() == 0)
                {
                    continue;
                }

                // chose the mosre similar candidate available
                ITreeNode<T> chosenCandidate = null;

                if (candidates.Count() == 1)
                {
                    chosenCandidate = candidates.First();
                }
                else
                {
                    chosenCandidate = child.MostSimilar(candidates);
                }

                // Add the intersection of child and candidate and add the result to the tree
                InternalAddChild(intersectionTree, child.Intersection(chosenCandidate, equality) as TreeNode<T>);
                testNodes.Remove(chosenCandidate);
            }

            return intersectionTree;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Difference(ITreeNode<T> other)
        {
            return Difference(other, UTITreeNode.BasicValueEquality<T>());
        }

        /// <inheritdoc />
        public override ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> travsersalEquality)
        {
            return Difference(other, travsersalEquality, (t1, t2) => true);
        }

        /// <inheritdoc />
        public override ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> travsersalEquality, DEquality<T> deletionEquality)
        {
            // nodes do not meet traveseral criteria, no further difference can be performed and
            // the original node value is returned. Difference ends here
            if (!travsersalEquality(this, other))
            {
                return new TreeNode<T>(Value);
            }

            // If there are no more children that could be removed, check if the node should be deleted.
            // if so delete, otherwise, keep
            if (ChildCount == 0)
            {
                return deletionEquality(Value, other.Value) ? null : new TreeNode<T>(Value);
            }

            // Make a tree with the current value
            TreeNode<T> differenceTree = new TreeNode<T>(Value);

            // test against the childrne of the other
            List<ITreeNode<T>> testNodes = new List<ITreeNode<T>>(other.Children);

            foreach (ITreeNode<T> child in Children)
            {
                // candidates for travseral match travseralEquality
                IEnumerable<ITreeNode<T>> candidates = testNodes.Where(n => travsersalEquality(n, child));

                // if there are no candidates, then copy this child to the difference tree
                // it is kept, and not deleted
                if (candidates == null || candidates.Count() == 0)
                {
                    child.Copy(differenceTree);
                    continue;
                }

                // find the candidate that requires traversal for more granular difference
                // This is the most similar node
                ITreeNode<T> chosenCandidate = null;

                if (candidates.Count() == 1)
                {
                    chosenCandidate = candidates.First();
                }
                else
                {
                    chosenCandidate = child.MostSimilar(candidates);
                }

                // Perform a difference on the current child and the chosen candidate
                TreeNode<T> possibleChild = child.Difference(chosenCandidate, travsersalEquality, deletionEquality) as TreeNode<T>;

                // If the child isn't null, add it to the difference tree
                if (possibleChild != null)
                {
                    InternalAddChild(differenceTree, possibleChild);
                }

                // remove candidate from future tests
                testNodes.Remove(chosenCandidate);
            }

            // if there are no children in the difference tree,  check if it should be deleted
            if (differenceTree.ChildCount == 0)
            {
                return deletionEquality(Value, other.Value) ? null : new TreeNode<T>(Value);
            }

            return differenceTree;
        }

        /// <inheritdoc />
        public override int Similarity(ITreeNode<T> other)
        {
            return Similarity(other, UTITreeNode.BasicValueEquality<T>());
        }

        /// <inheritdoc />
        public override int Similarity(ITreeNode<T> other, DEquality<ITreeNode<T>> equality)
        {
            // if equal, give one similarity point
            int similarity = equality(this, other) ? 1 : 0;

            // Find out how many children are similar
            int sharedChildren = UTIEquatable.SharedValuesCount<T>(
                Children.Select(n => n.Value), other.Children.Select(n => n.Value)
            );

            // remove similarity points based on non-shared children
            similarity -= (ChildCount - sharedChildren);
            similarity -= (other.ChildCount - sharedChildren);

            // Use other children as a test of similarity
            List<ITreeNode<T>> testNodes = new List<ITreeNode<T>>(other.Children);

            foreach (ITreeNode<T> child in Children)
            {
                // determine the pairedNode (Node that is the equivalent of the this node)
                ITreeNode<T> pairedNode = null;

                // Find the nodes that are considered equal
                IEnumerable<ITreeNode<T>> possibleTestNodes = testNodes.Where(n => equality(n, child));
                int count = possibleTestNodes.Count();

                if (count > 1)
                {
                    // if there is more than one possible test node, do a similarity test on each
                    // and set paired node to the most similar. 
                    int innerSimilarity = int.MinValue;

                    foreach (ITreeNode<T> node in possibleTestNodes)
                    {
                        int sim = child.Similarity(node, equality);

                        if (sim > innerSimilarity)
                        {
                            innerSimilarity = sim;
                            pairedNode = node;
                        }
                    }

                    // Add the innerSimilarity to the similarity calculation
                    similarity += innerSimilarity;
                }
                else if (count == 1)
                {
                    // if there is only one, then add the similarty of these nodes
                    // and remove the test node
                    ITreeNode<T> testNode = possibleTestNodes.First();
                    similarity += child.Similarity(testNode, equality);
                    pairedNode = testNode;
                }

                // remove candidate
                if (pairedNode != null)
                {
                    testNodes.Remove(pairedNode);
                }
            }

            // All lingering nodes are subtracted from similarity
            foreach (ITreeNode<T> node in testNodes)
            {
                similarity -= node.DescendentCount;
            }

            return similarity;
        }

        /// <inheritdoc />
        public override IEnumerator<ITreeNode<T>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Unsafe add child adds a child directly to the parent, then changes
        /// the parent of the child. This targets private variables and, if implemented
        /// incorrectly will cause major issues. 
        /// </summary>
        /// <param name="parent">The parent of the node to add</param>
        /// <param name="child">The child to add to the parent</param>
        private void InternalAddChild(TreeNode<T> parent, TreeNode<T> child)
        {
            parent._children.Add(child);
            child._parent = parent;
        }
    }
}
