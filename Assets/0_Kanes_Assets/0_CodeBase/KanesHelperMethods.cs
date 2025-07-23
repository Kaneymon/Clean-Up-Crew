using System.Collections.Generic;
using System;
using UnityEngine;

public static class KanesHelperMethods
{
    public static void RemoveAllFromStack<T>(Stack<T> stack, Predicate<T> match)
    {
        Stack<T> tempStack = new Stack<T>();
        while (stack.Count > 0)
        {
            T current = stack.Pop();
            if (!match(current))
            {
                tempStack.Push(current);
            }
        }

        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }

    public static void RemoveFromStack<T>(Stack<T> stack, T target)
    {
        Stack<T> tempStack = new Stack<T>();
        while (stack.Count > 0)
        {
            T current = stack.Pop();
            if (!EqualityComparer<T>.Default.Equals(current, target))
            {
                tempStack.Push(current);
            }
        }

        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }
}
