using System;

namespace BinaryTree
{
    public class Tree
    {

        public Node root = null;

        public void insert(int key, int val)
        {

            if (root == null)
            {
                root = new Node(key, val);
                return;
            }

            Node current = root;

            Node parent = null;

            while (current != null)
            {
                if (key == current.key) return;
                parent = current;
                if (current.key < key) current = current.right;
                else current = current.left;
            }

            try
            {
                lock (parent)
                {
                    Node newNode = new Node(key, val);
                    newNode.parent = parent;
                    if (parent.key > newNode.key) parent.left = newNode;
                    else parent.right = newNode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void delete(int key)
        {
            deleteNode(SearchNode(key));
        }

        void deleteNode(Node node)
        {
            
            if (root == null) return;

            if (node == root && node.right == null && node.left == null) root = null;
            
            Node current = root;

            try
            {
                lock (node)
                {
                    while (current != null)
                    {
                        if (current.key == node.key)
                        {
                            if (node.right == null && node.left == null)
                            {
                                if (node.parent.right == node) node.parent.right = null;
                                else node.parent.left = null;
                            }

                            if (node.left == null && node.right != null)
                            {
                                lock (node.right)
                                {
                                    if (node.parent != null)
                                    {
                                        node.right.parent = node.parent;
                                        if (node.parent.right == node) node.parent.right = node.right;
                                        else node.parent.left = node.right;
                                    }
                                    else
                                    {
                                        node.right.parent = null;
                                        root = node.right;
                                    }
                                }
                            }

                            if (node.left != null && node.right == null)
                            {
                                lock (node.left)
                                {
                                    if (node.parent != null)
                                    {
                                        node.left.parent = node.parent;
                                        if (node.parent.left == node) node.parent.left = node.left;
                                        else node.parent.right = node.left;
                                    }
                                    else
                                    {
                                        node.left.parent = null;
                                        root = node.left;
                                    }
                                }
                            }

                            if (node.left != null && node.right != null)
                            {
                                Node leftest = node.right;
                                while (leftest.left != null)
                                {
                                    leftest = leftest.left;
                                }
                                lock (leftest)
                                {
                                    node.key = leftest.key;
                                    node.data = leftest.data;
                                    deleteNode(leftest);
                                }
                            }
                        }

                        if (current.key > node.key) current = current.left;
                        else current = current.right;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        int GetNodeHeight(Node node)
        {
            Node current = node;
            int height = 0;

            while (current != null)
            {
                current = current.parent;
                height++;
            }

            return height;
        }
        
        internal void PrintTree (Node node)
        {
            if (root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }
            if (node.right != null) PrintTree(node.right);

            for (int i = 0; i < GetNodeHeight(node); i++)
                Console.Write("  ");
            if (node.parent != null)
            {
                if (node == node.parent.right)
                    Console.Write("/");
                else Console.Write("\\");
            }
            
            Console.WriteLine(node.data);
            

            if (node.left != null) PrintTree(node.left);           
        }

        public Node SearchNode(int key)
        {
            if (root == null) return null;
            
            if (root.key == key) return root;

            Node current = root;

            while (current != null)
            {
                if (current.key == key)
                {
                    try
                    {
                        lock (current)
                        {
                            return (current);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        continue;
                    }
                }

                if (current.key > key) current = current.left;
                else current = current.right;
            }

            return null;

        }

    }
}
