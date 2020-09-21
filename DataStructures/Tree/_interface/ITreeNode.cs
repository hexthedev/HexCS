using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// Tree nodes are a single node within a tree. They contain a value and can be
    /// traversed as a tree data structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeNode<T> : IEnumerable<ITreeNode<T>>, IEquatable<ITreeNode<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Tree the node is contained in
        /// </summary>
        ITreeNode<T> Root { get; }

        /// <summary>
        /// The Parent node of the tree
        /// </summary>
        ITreeNode<T> Parent { get; }

        /// <summary>
        /// Get children as IEnumerable
        /// </summary>
        IEnumerable<ITreeNode<T>> Children { get; }

        /// <summary>
        /// The value of the node
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Is this node the root of the tree
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Is this node a leaf in the tree
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets full count count of all nodes in tree starting at this. 
        /// </summary>
        int DescendentCount { get; }

        /// <summary>
        /// The number of children this node has
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// Get the depth of the node
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// Add a child to the tree node
        /// </summary>
        /// <param name="value">Value of child to add</param>
        ITreeNode<T> AddChild(T value);

        /// <summary>
        /// Add a multiple children to this node
        /// </summary>
        /// <param name="values">Valud of children to add</param>
        void AddChildren(params T[] values);

        /// <summary>
        /// Get child at index
        /// </summary>
        /// <param name="index">index of child to get</param>
        /// <returns></returns>
        ITreeNode<T> GetChild(int index);

        /// <summary>
        /// Remove child at index
        /// </summary>
        /// <param name="index">index of child to remove</param>
        /// <returns></returns>
        void RemoveChild(int index);

        /// <summary>
        /// Remove node using reference
        /// </summary>
        /// <param name="node">reference to child to remove</param>
        void RemoveChild(ITreeNode<T> node);

        /// <summary>
        /// Change the value of the current node, return true if successful
        /// </summary>
        /// <param name="value">Set value of this node</param>
        bool SetValue(T value);

        /// <summary>
        /// Recursively copy a tree to a new parent
        /// </summary>
        /// <param name="newParent">newParent to copy tree to</param>
        /// <returns>new node created under new parent</returns>
        ITreeNode<T> Copy(ITreeNode<T> newParent = null);

        /// <summary>
        /// Returns enumerator which moves through children of node in pattern
        /// based on ETreeSearchType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ITreeNode<T>> SearchEnumeration(ETreeSearchType type);

        /// <summary>
        /// Take both trees and returns a new tree containing all paths 
        /// from both
        /// </summary>
        /// <param name="other">other node</param>
        /// <returns>new tree</returns>
        ITreeNode<T> Union(ITreeNode<T> other);

        /// <summary>
        /// Take both trees and returns a new tree containing all paths 
        /// from both
        /// </summary>
        /// <param name="other">other node</param>
        /// <param name="equality">function that acts as equality test</param>
        /// <returns>new tree</returns>
        ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);   // Use custom equality function

        /// <summary>
        /// Take both trees and returns a new tree containing all paths 
        /// from both
        /// </summary>
        /// <param name="other">other node</param>
        /// <param name="equality">function that acts as equality test</param>
        /// <param name="merge">function that determines how values should be merged when equality = true</param>
        /// <returns>new tree</returns>
        ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality, DMerge<T> merge);   // Use custom merge on matching values

        /// <summary>
        /// Takes both trees and returns a new tree containing only paths
        /// each shares
        /// </summary>
        /// <param name="other">other node</param>
        /// <returns>new node</returns>
        ITreeNode<T> Intersection(ITreeNode<T> other);

        /// <summary>
        /// Takes both trees and returns a new tree containing only paths
        /// each shares
        /// </summary>
        /// <param name="other">other node</param>
        /// <param name="equality">function that acts as equality test</param>
        /// <returns>new node</returns>
        ITreeNode<T> Intersection(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);

        /// <summary>
        /// Takes a tree and removes all paths shared with tree other
        /// </summary>
        /// <param name="other">other node</param>
        /// <returns>new node</returns>
        ITreeNode<T> Difference(ITreeNode<T> other);

        /// <summary>
        /// Takes a tree and removes all paths shared with tree other
        /// </summary>
        /// <param name="other">other node</param>
        /// <param name="traversalEquality">Equality test used to determine nodes to traverse</param>
        /// <returns>new node</returns>
        ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> traversalEquality);

        /// <summary>
        /// Takes a tree and removes all paths shared with tree other
        /// </summary>
        /// <param name="other">other node</param>
        /// <param name="traversalEquality">Determines whether another node should be traversed to determine a more granular deletion of internal paths</param>
        /// <param name="deletionEquality">Determines whether another value is equal, and therefore should cause a deletion</param>
        /// <returns>new node</returns>
        ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> traversalEquality, DEquality<T> deletionEquality);

        /// <summary>
        /// Returns a number representing the number of similar paths
        /// this node has to another. More similar nodes have more shared paths
        /// </summary>
        /// <param name="other">node to compare with</param>
        /// <returns>mesure of similarity</returns>
        int Similarity(ITreeNode<T> other);

        /// <summary>
        /// Returns a number representing the number of similar paths
        /// this node has to another. More similar nodes have more shared paths
        /// </summary>
        /// <param name="other">node to compare with</param>
        /// <param name="equality">mesure of equality</param>
        /// <returns>mesure of similarity</returns>
        int Similarity(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);
    }
}
