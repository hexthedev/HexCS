using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// Returns an enumerator that traverses a tree in a BFS style. This means
    /// return all nodes of a particular depth before moving to the next
    /// depth
    /// </summary>
    /// <typeparam name="T">Tree type</typeparam>
    public class BreathFirstSearchEnumerator<T> : IEnumerator<TreeNode<T>> where T : IEquatable<T>
    {
        private Queue<TreeNode<T>> _queue;

        private TreeNode<T> _current;

        #region API
        /// <inheritdoc />
        public object Current => _current;

        /// <summary>
        /// Create an BFS enumerator from the startNode
        /// </summary>
        /// <param name="startNode">node to start search from</param>
        public BreathFirstSearchEnumerator(TreeNode<T> startNode)
        {
            _current = startNode;
        }

        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public bool MoveNext()
        {
            // if first call, make queue and queue root
            if (_queue == null)
            {
                _queue = new Queue<TreeNode<T>>();
                _queue.Enqueue(_current);
            }

            // If there is nothing left in queue, end of enumerator
            if (_queue.Count == 0)
            {
                _current = null;
                return false;
            }

            TreeNode<T> node = _queue.Dequeue();

            // if node isn't null, add children to queue
            if (node != null)
            {
                foreach (TreeNode<T> child in node)
                {
                    _queue.Enqueue(child);
                }
            }

            _current = node;
            return true;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _queue = null;
            _current = null;
        }
        #endregion

        TreeNode<T> IEnumerator<TreeNode<T>>.Current { get { return _current; } }
    }
}