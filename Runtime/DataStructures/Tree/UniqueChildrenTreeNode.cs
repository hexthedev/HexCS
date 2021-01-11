using System;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// A derivative of tree that can only contain tree nodes that have unqiue values
    /// as neighbours
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UniqueChildrenTreeNode<T> : TreeNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Construct node with value
        /// </summary>
        /// <param name="value"></param>
        public UniqueChildrenTreeNode(T value) : base(value) { }

        /// <summary>
        /// COnstruct node with default value
        /// </summary>
        public UniqueChildrenTreeNode() : base() { }

        /// <summary>
        /// Adds child and returns node if child with value does not already exist
        /// Otherwise returns null
        /// </summary>
        /// <param name="value">value of node</param>
        /// <returns>created child or null</returns>
        public override ITreeNode<T> AddChild(T value)
        {
            if (_children.QueryContains(n => { return n.Value.Equals(value); }))
            {
                return null;
            }

            UniqueChildrenTreeNode<T> node = new UniqueChildrenTreeNode<T>(value);
            UnsafeAddChild(this, node);
            return node;
        }

        /// <summary>
        /// Sets value or resturns false if value is not unique to siblings
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>false if value exists in sibling</returns>
        public override bool SetValue(T value)
        {
            if (Parent != null && Parent.Children.QueryContains(n => n.Value.Equals(value)))
            {
                return false;
            }

            Value = value;
            return true;
        }

        /// <inheritdoc />
        public override int Similarity(ITreeNode<T> other, DEquality<ITreeNode<T>> equality)
        {
            int similarity = equality(this, other) ? 1 : 0;

            List<ITreeNode<T>> nodes = new List<ITreeNode<T>>(other.Children);

            foreach (ITreeNode<T> child in Children)
            {
                ITreeNode<T> node = nodes.Find(n => equality(n, child));

                // if node is default then no equal node exists. subtract from similarity
                if (node == default(ITreeNode<T>))
                {
                    similarity -= (1 + child.DescendentCount);
                }
                // Otherwise, add the similarity of the new node
                else
                {
                    similarity += child.Similarity(node);
                    nodes.Remove(node);
                }
            }

            // subtract remaining
            foreach (ITreeNode<T> child in nodes)
            {
                similarity -= (1 + child.DescendentCount);
            }

            return similarity;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality, DMerge<T> merge)
        {
            // if not equal, return null
            if (!equality(this, other))
            {
                return null;
            }

            // since values are equal, if they both have no children, return node with value
            if (ChildCount == 0 && other.ChildCount == 0)
            {
                return new UniqueChildrenTreeNode<T>(merge(Value, other.Value));
            }

            // return copy of node with children if only 1 has children
            if (ChildCount > 0 && other.ChildCount == 0)
            {
                return Copy();
            }

            if (other.ChildCount > 0 && ChildCount == 0)
            {
                return other.Copy();
            }

            // set up the union tree for recursive calculation
            UniqueChildrenTreeNode<T> unionTree = new UniqueChildrenTreeNode<T>(merge(Value, other.Value));

            // sort out children in both that have no equivalent in other
            // perform a union on any nodes that match
            List<ITreeNode<T>> otherNodesNotInOriginal = new List<ITreeNode<T>>(other.Children);
            List<ITreeNode<T>> originalNodesNotInOther = new List<ITreeNode<T>>();

            foreach (ITreeNode<T> child in Children)
            {
                ITreeNode<T> matchingOtherNode = otherNodesNotInOriginal.Find(n => equality(n, child));

                if (matchingOtherNode == default(ITreeNode<T>))
                {
                    originalNodesNotInOther.Add(child);
                }
                else
                {
                    UniqueChildrenTreeNode<T> newUnion
                        = child.Union(matchingOtherNode) as UniqueChildrenTreeNode<T>;

                    if (newUnion == null)
                    {
                        continue;
                    }

                    // add union of matching nodes to tree
                    UnsafeAddChild(unionTree, newUnion);
                    otherNodesNotInOriginal.Remove(matchingOtherNode);
                }
            }

            // copy other nodes missing from original
            foreach (ITreeNode<T> node in otherNodesNotInOriginal)
            {
                node.Copy(unionTree);
            }

            // copy original nodes missing from other
            foreach (ITreeNode<T> node in originalNodesNotInOther)
            {
                node.Copy(unionTree);
            }

            return unionTree;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Intersection(ITreeNode<T> other, DEquality<ITreeNode<T>> equality)
        {
            // if not equal return null
            if (!equality(this, other))
            {
                return null;
            }

            // if netiehr have children return new tree node
            if (ChildCount == 0 || other.ChildCount == 0)
            {
                return new UniqueChildrenTreeNode<T>(Value);
            }

            // process node and children to find intersection
            UniqueChildrenTreeNode<T> intersectionRoot = new UniqueChildrenTreeNode<T>(Value);

            List<ITreeNode<T>> otherChildren = new List<ITreeNode<T>>(other.Children);

            foreach (ITreeNode<T> originalChild in Children)
            {
                ITreeNode<T> matchingOtherNode = otherChildren.Find(n => equality(n, originalChild));

                // if a match exists, perform an intersection
                if (matchingOtherNode != default(ITreeNode<T>))
                {
                    UniqueChildrenTreeNode<T> newIntersection
                        = originalChild.Intersection(matchingOtherNode) as UniqueChildrenTreeNode<T>;

                    if (newIntersection == null)
                    {
                        continue;
                    }

                    UnsafeAddChild(intersectionRoot, newIntersection);
                    otherChildren.Remove(matchingOtherNode);
                }
            }
            return intersectionRoot;
        }

        /// <inheritdoc />
        public override ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> traversalEquality, DEquality<T> deletionEquality)
        {
            // if nodes arn't equal return nothing
            if (Equals(other))
            {
                return null;
            }

            // if these arn't traversal equal, return a copy of this node
            if (!traversalEquality(this, other))
            {
                return Copy();
            }

            // if this has no children, test for deletion
            if (ChildCount == 0)
            {
                return deletionEquality(Value, other.Value) ? null : new UniqueChildrenTreeNode<T>(this.Value);
            }

            // if other has no children, retun this as copy
            if (other.ChildCount == 0)
            {
                return Copy();
            }

            // calculate difference of children nodes
            UniqueChildrenTreeNode<T> differenceTree = new UniqueChildrenTreeNode<T>(Value);

            List<ITreeNode<T>> originalNodesWithoutTraversalNeighbour = new List<ITreeNode<T>>(Children);

            foreach (ITreeNode<T> child in other.Children)
            {
                ITreeNode<T> traversalNode = originalNodesWithoutTraversalNeighbour.Find(n => traversalEquality(n, child));

                // if node exists, perform a difference and add new node to output
                if (traversalNode != default(ITreeNode<T>))
                {
                    UniqueChildrenTreeNode<T> newDifference
                        = traversalNode.Difference(child, traversalEquality, deletionEquality) as UniqueChildrenTreeNode<T>;

                    originalNodesWithoutTraversalNeighbour.Remove(traversalNode);

                    if (newDifference == null)
                    {
                        continue;
                    }

                    UnsafeAddChild(differenceTree, newDifference);
                }
            }

            // Add all nodes without a traversale neighbour to output
            foreach (ITreeNode<T> node in originalNodesWithoutTraversalNeighbour)
            {
                node.Copy(differenceTree);
            }

            return differenceTree;
        }

        private void UnsafeAddChild(UniqueChildrenTreeNode<T> parent, UniqueChildrenTreeNode<T> child)
        {
            parent._children.Add(child);
            child._parent = parent;
        }
    }
}
