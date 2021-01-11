using System;
using System.Collections;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Abstract tree class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ATreeNodeBase<T> : ITreeNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Parent of node
        /// </summary>
        protected ATreeNodeBase<T> _parent;

        /// <summary>
        /// Children of node
        /// </summary>
        protected List<ATreeNodeBase<T>> _children = new List<ATreeNodeBase<T>>();

        #region API
        /// <inheritdoc />
        public ITreeNode<T> Parent => _parent;

        /// <inheritdoc />
        public IEnumerable<ITreeNode<T>> Children => _children;

        /// <inheritdoc />
        public abstract ITreeNode<T> Root { get; }

        /// <inheritdoc />
        public abstract bool IsRoot { get; }

        /// <inheritdoc />
        public abstract T Value { get; protected set; }

        /// <inheritdoc />
        public abstract bool IsLeaf { get; }

        /// <inheritdoc />
        public abstract int DescendentCount { get; }
        
        /// <inheritdoc />
        public abstract int ChildCount { get; }

        /// <inheritdoc />
        public abstract int Depth { get; }

        /// <inheritdoc />
        public abstract ITreeNode<T> AddChild(T value);

        /// <inheritdoc />
        public abstract void AddChildren(params T[] values);

        /// <inheritdoc />
        public abstract ITreeNode<T> Copy(ITreeNode<T> newParent = null);

        /// <inheritdoc />
        public abstract ITreeNode<T> GetChild(int index);

        /// <inheritdoc />
        public abstract IEnumerator<ITreeNode<T>> GetEnumerator();

        /// <inheritdoc />
        public abstract void RemoveChild(int index);

        /// <inheritdoc />
        public abstract void RemoveChild(ITreeNode<T> node);

        /// <inheritdoc />
        public abstract IEnumerable<ITreeNode<T>> SearchEnumeration(ETreeSearchType type);

        /// <inheritdoc />
        public abstract bool SetValue(T value);

        /// <inheritdoc />
        public abstract ITreeNode<T> Union(ITreeNode<T> other);

        /// <inheritdoc />
        public abstract ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);

        /// <inheritdoc />
        public abstract ITreeNode<T> Union(ITreeNode<T> other, DEquality<ITreeNode<T>> equality, DMerge<T> merge);

        /// <inheritdoc />
        public abstract ITreeNode<T> Intersection(ITreeNode<T> other);

        /// <inheritdoc />
        public abstract ITreeNode<T> Intersection(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);

        /// <inheritdoc />
        public abstract ITreeNode<T> Difference(ITreeNode<T> other);
        /// <inheritdoc />
        public abstract ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> traversal_equality);
        /// <inheritdoc />
        public abstract ITreeNode<T> Difference(ITreeNode<T> other, DEquality<ITreeNode<T>> traversal_equality, DEquality<T> deletion_equality);

        /// <inheritdoc />
        public abstract int Similarity(ITreeNode<T> other);
        /// <inheritdoc />
        public abstract int Similarity(ITreeNode<T> other, DEquality<ITreeNode<T>> equality);

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (!(other is ITreeNode<T>))
            {
                return false;
            }

            ITreeNode<T> other_caster = other as ITreeNode<T>;

            return Equals(other_caster);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = UTHash.BasicHash(Value);

            List<ITreeNode<T>> nodes = new List<ITreeNode<T>>(_children);
            nodes.Sort((n1, n2) => UTHash.HashComparison(n1.Value, n2.Value));

            foreach (ITreeNode<T> child in nodes)
            {
                hash += child.GetHashCode();
            }

            return hash;
        }

        /// <inheritdoc />
        public bool Equals(ITreeNode<T> other)
        {
            if (other == null)
            {
                return false;
            }

            if (!other.Value.Equals(Value))
            {
                return false;
            }

            if (other.ChildCount != ChildCount)
            {
                return false;
            }

            return this.GetHashCode() == other.GetHashCode();
        }
        #endregion

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }
    }
}
