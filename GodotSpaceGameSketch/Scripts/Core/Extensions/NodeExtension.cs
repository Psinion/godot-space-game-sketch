using System.Collections.Generic;
using Godot;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class NodeExtension
{
    /**
     * Get all nodes of T type in children
     */
    public static T[] GetNodeInChildren<T>(this Node node, bool recursive = false) where T : Node
    {
        var nodes = new List<T>();
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            if (node.GetChild(i) is T)
            {
                nodes.Add((T)node.GetChild(i));
            }
            else
            {
                if (!recursive)
                {
                    continue;
                }
                var childResult = node.GetChild(i).GetNodeInChildren<T>(true);
                nodes.AddRange(childResult);
            }
        }
		
        return nodes.ToArray();
    }
    
    public static bool TryGetNode<T>(this Node n, out T node, bool recursive = false) where T : Node
    {
        for (int i = 0; i < n.GetChildCount(); i++)
        {
            if (n.GetChild(i) is T)
            {
                node = (T)n.GetChild(i);
                return true;
            }

            if (recursive)
            {
                if (n.GetChild(i).TryGetNode(out T recurseResult, true))
                {
                    node = recurseResult;
                    return true;
                }
            }
        }

        node = null;
        return false;
    }
    
    /**
     * Get child without exceptions
     */
    public static bool TryGetChild(this Node parent, int index, out Node child)
    {
        child = null;
        if (parent.GetChildCount() > index)
        {
            child = parent.GetChild(index);
            return true;
        }
        return false;
    }
}