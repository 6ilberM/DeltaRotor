using System.Collections;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{

    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T FirstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return FirstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int ChildIndexLeft = item.heapIndex * 2 + 1;
            int ChildIndexRight = item.heapIndex * 2 + 2;
            int Swapindex = 0;

            if (ChildIndexLeft < currentItemCount)
            {
                Swapindex = ChildIndexLeft;

                if (ChildIndexRight < currentItemCount)
                {
                    if (items[ChildIndexLeft].CompareTo(items[ChildIndexRight]) < 0)
                    {
                        Swapindex = ChildIndexRight;
                    }
                }

                if (item.CompareTo(items[Swapindex]) < 0)
                {
                    Swap(item, items[Swapindex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;

        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}
