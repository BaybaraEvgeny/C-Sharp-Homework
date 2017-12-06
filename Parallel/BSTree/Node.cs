
namespace BinaryTree
{
    public class Node
    {
        public int key;
        public int data;

        public Node parent;
        public Node left;
        public Node right;

        public Node(int key, int data)
        {
            this.key = key;
            this.data = data;
            this.parent = null;
            this.left = null;
            this.right = null;
        }
        
    }
}
