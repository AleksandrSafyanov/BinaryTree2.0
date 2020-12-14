using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTree
{
  class BTree
  {
    int LastLvl;
    int FixLvl;
    int Count;
    List<int[]> tree = new List<int[]>();
    static int countTempStor = 20;
    int[] tempStor = new int[countTempStor];
    int position;
    int countInsertAsRoot;
    int[] DataInsertAsRoot = new int[countTempStor];

    public BTree()
    {
      FixLvl = 0;
      LastLvl = 0;
      tree.Add(new int[1]);
      Count = 0;
      countInsertAsRoot = 0;
      position = 0;
    }
    public void Insert(int addData)
    {
      Insert(addData, 0, 0, tree);
    }
    public void InsertNotRandom(int addData)
    {
      InsertNotRandom(addData, 0, 0, tree);
    }
    public void MergeTrees(BTree A, BTree B)
    {
      B.GetTree(0, 0, B.tree, A);
    }

    void InsertNotRandom(int addData, int elem, int lvl, List<int[]> leaf)
    {
      if (leaf[lvl][elem] != 0 && leaf[lvl][elem] != -1)
      {
        if (lvl == LastLvl)
        {
          leaf.Add(new int[((int)Math.Pow(2, lvl + 1))]);
          LastLvl++;
        }

        if (addData < leaf[lvl][elem])
          InsertNotRandom(addData, ((elem + 1) * 2) - 2, lvl + 1, leaf);
        else
          InsertNotRandom(addData, ((elem + 1) * 2) - 1, lvl + 1, leaf);
      }
      else
      {
        if (elem % 2 == 0)
        {
          leaf[lvl][elem] = addData;
        }
        else
        {
          if (leaf[lvl][elem - 1] == 0 || leaf[lvl][elem - 1] == -1)
          {
            leaf[lvl][elem - 1] = -1;
          }
          
          leaf[lvl][elem] = addData;
        }
      }
    }
    void Insert(int addData, int elem, int lvl, List<int[]> leaf)
    {
      Random rnd = new Random();

      if (leaf[lvl][elem] != 0 && leaf[lvl][elem] != -1)
      {
        if (lvl == LastLvl)
        {
          leaf.Add(new int[((int)Math.Pow(2, lvl + 1))]);
          LastLvl++;
        }

        if (rnd.Next() % (Count + 1) == 0)
        {
          InsertRoot(addData, elem, lvl, leaf);
          DataInsertAsRoot[countInsertAsRoot] = addData; 
          countInsertAsRoot++;
          Count++;
          return;
        }

        if (addData < leaf[lvl][elem])
          Insert(addData, ((elem + 1) * 2) - 2, lvl + 1, leaf);
        else
          Insert(addData, ((elem + 1) * 2) - 1, lvl + 1, leaf);
      }
      else
      {
        if (elem % 2 == 0)
        {
          leaf[lvl][elem] = addData;
          Count++;
        }
        else
        {
          if (leaf[lvl][elem - 1] == 0 || leaf[lvl][elem - 1] == -1)
          {
            leaf[lvl][elem - 1] = -1;
          }

          leaf[lvl][elem] = addData;
          Count++;
        }
      }
    }

    void InsertRoot(int addData, int elem, int lvl, List<int[]> leaf)
    {

      if (leaf[lvl][elem] != 0 && leaf[lvl][elem] != -1)
      {
        if (lvl == LastLvl)
        {
          leaf.Add(new int[((int)Math.Pow(2, lvl + 1))]);
          LastLvl++;
        }
        if (addData < leaf[lvl][elem])
        {
          InsertRoot(addData, ((elem + 1) * 2) - 2, lvl + 1, leaf);
          RotateRight(elem, lvl, leaf);
        }
        else
        {
          InsertRoot(addData, ((elem + 1) * 2) - 1, lvl + 1, leaf);
          RotateLeft(elem, lvl, leaf);
        }
      }
      else
      {
        leaf[lvl][elem] = addData;
      }
    }
    void RotateRight(int elem, int lvl, List<int[]> leaf)
    {
      
      if (leaf[lvl][elem] != 0)
      {
        if (lvl == LastLvl)
        {
          leaf.Add(new int[((int)Math.Pow(2, lvl + 1))]);
          LastLvl++;
        }
        int temp = leaf[lvl + 1][((elem + 1) * 2) - 2];
        leaf[lvl + 1][((elem + 1) * 2) - 2] = leaf[lvl][elem];
        leaf[lvl][elem] = temp;

        AddInArray(elem, lvl, leaf);
        position = 0;
        //PrintTempStor();
        TempDeleteTree(elem, lvl, leaf);

        int countOfArray = GetNumberOfArray();
        //Console.WriteLine(countOfArray);

        for (int i = 0; i < countOfArray; i++)
        {
          InsertNotRandom(tempStor[i], 0, 0, leaf);
        }

        ClearArray();
      }
    }
    void RotateLeft(int elem, int lvl, List<int[]> leaf)
    {
      
      if (leaf[lvl][elem] != 0)
      {
        if (lvl == LastLvl)
        {
          leaf.Add(new int[((int)Math.Pow(2, lvl + 1))]);
          LastLvl++;
        }
        int temp = leaf[lvl + 1][((elem + 1) * 2) - 1];
        leaf[lvl + 1][((elem + 1) * 2) - 1] = leaf[lvl][elem];
        leaf[lvl][elem] = temp;

        AddInArray(elem, lvl, leaf);
        position = 0;
        //PrintTempStor();
        TempDeleteTree(elem, lvl, leaf);

        int countOfArray = GetNumberOfArray();
        //Console.WriteLine(countOfArray);

        for (int i = 0; i < countOfArray; i++)
        {
          InsertNotRandom(tempStor[i], 0, 0, leaf);
        }

        ClearArray();
      }
    }

    void AddInArray(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      if (leaf[lvl][elem] == 0)
        return;

      if (lvl + 1 == LastLvl)
      {
        if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
        {
          tempStor[position] = leaf[level][leftChild];
          position++;
        }

        if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
        {
          tempStor[position] = leaf[level][rightChild];
          position++;
        }

      }
      else
      {
        if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
        {
          tempStor[position] = leaf[level][leftChild];
          position++;
          AddInArray(leftChild, level, leaf);
        }
        if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
        {
          tempStor[position] = leaf[level][rightChild];
          position++;
          AddInArray(rightChild, level, leaf);
        }
      }
    }
    void TempDeleteTree(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      if (leaf[lvl][elem] == 0)
        return;

      if (lvl + 1 == LastLvl)
      {
        if (leaf[level][leftChild] != 0)
          leaf[level][leftChild] = 0;
        if (leaf[level][rightChild] != 0)
          leaf[level][rightChild] = 0;
      }
      else
      {
        if (leaf[level][leftChild] != 0)
        {
          TempDeleteTree(leftChild, level, leaf);
          leaf[level][leftChild] = 0;
        }
        if (leaf[level][rightChild] != 0)
        {
          TempDeleteTree(rightChild, level, leaf);
          leaf[level][rightChild] = 0;
        }
      }
    }
    int GetNumberOfArray()
    {
      int countOfArray = 0;
      for (int i = 0; i < countTempStor; i++)
      {
        if (tempStor[i] != 0)
        {
          countOfArray++;
        }
      }
      return countOfArray;
    }
    void ClearArray()
    {
      for (int j = 0; j < countTempStor; j++)
        tempStor[j] = 0;
    }

    void GetTree(int elem, int lvl, List<int[]> leafB, BTree leafA)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      leafA.Insert(leafB[lvl][elem]);

      if (lvl != LastLvl)
      {
        if (leafB[level][leftChild] != 0 && leafB[level][leftChild] != -1)
        {
          GetTree(leftChild, level, leafB, leafA);
        }
        if (leafB[level][rightChild] != 0 && leafB[level][rightChild] != -1)
        {
          GetTree(rightChild, level, leafB, leafA);
        }
      }
    }

    public void FixTree()
    {
      FixTree(0, 0, tree);
      FixLevel();
    }
    void FixTree(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      if (FixLvl == lvl)
        FixLvl++;

      if (leaf[lvl][elem] == 0)
        return;

      if (lvl + 1 == LastLvl)
      {
        if ((leaf[level][leftChild] == 0 || leaf[level][leftChild] == -1) && (leaf[level][rightChild] == 0 || leaf[level][rightChild] == -1))
        {
          leaf[level][leftChild] = 0;
          leaf[level][rightChild] = 0;
        }
      }
      else
      {
        if ((leaf[level][leftChild] == 0 || leaf[level][leftChild] == -1) && (leaf[level][rightChild] == 0 || leaf[level][rightChild] == -1))
        {
          leaf[level][leftChild] = 0;
          leaf[level][rightChild] = 0;
        }
        else
        {
          if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
          {
            FixTree(leftChild, level, leaf);
          }

          if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
          {
            FixTree(rightChild, level, leaf);
          }
        }
      }
    }
    void FixLevel()
    {
      if (FixLvl == LastLvl)
        LastLvl = FixLvl;
    }

    public void PrintCount()
    {
      Console.WriteLine();
      Console.WriteLine("Кол-во элементов = " + Count);
    }
    public void PrintLvl()
    {
      Console.WriteLine();
      Console.WriteLine("Последний уровень = " + (LastLvl + 1));
    }
    public void PrintCountInsertAsRoot()
    {
      Console.WriteLine();
      Console.WriteLine("Кол-во постановок в корень = " + countInsertAsRoot);
    }
    public void PrintDataInsertAsRoot()
    {
      Console.WriteLine();
      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Значения поставленные в корень:");
      for (int i = 0; i < countTempStor; i++)
      {
        if (DataInsertAsRoot[i] != 0)
        {
          Console.Write(DataInsertAsRoot[i] + " ");
        }
      }
      Console.WriteLine("\n──────────────────────────────");
    }
    public void PrintTempStor()
    {
      for (int i = 0; i < countTempStor; i++)
        Console.WriteLine("Temp [" + (i + 1) + "] = " + tempStor[i] + " ");
    }

    public void PrintList()
    {
      PrintListTree(LastLvl + 1);

      Console.WriteLine();
    }
    void PrintListTree(int level)
    {
      int before, after, rate;
      int beforeLine;
      for (int i = 0; i < level; i++)
      {
        rate = 2;
        before = ((int)Math.Pow(2, level - i) / rate);
        beforeLine = ((int)Math.Pow(2, level - i - 1) / rate);
        after = before * 2 - 1;
        for (int k = 0; k < before; k++)
        {
          Console.Write("  ");
        }

        for (int j = 0; j < (int)Math.Pow(2, i); j++)
        {
          if (j == (int)Math.Pow(2, i) - 1)
          {
            if (tree[i][j] == 0)
              Console.Write("  ");
            else
            {
              SwapColors();
              if (tree[i][j] < 10)
              {
                if (tree[i][j] < 0)
                {
                  Console.Write(tree[i][j]);
                }
                else
                {
                  Console.Write("0");
                  Console.Write(tree[i][j]);
                }
              }
              else
                Console.Write(tree[i][j]);
              SwapColors();
            }
          }
          else
          {
            if (tree[i][j] == 0)
              Console.Write("  ");
            else
            {
              SwapColors();
              if (tree[i][j] < 10)
              {
                if (tree[i][j] < 0)
                {
                  Console.Write(tree[i][j]);
                }
                else
                {
                  Console.Write("0");
                  Console.Write(tree[i][j]);
                }
              }
              else
                Console.Write(tree[i][j]);
              SwapColors();
            }
            for (int k = 0; k < after; k++)
            {
              Console.Write("  ");
            }
          }
        }
        for (int z = 0; z < after; z++)
        {
          Console.Write(" ");
        }
        Console.Write("|");
        if (i != level - 1)
        {
          Console.WriteLine();
          for (int k = 0; k < beforeLine; k++)
          {
            Console.Write("  ");
          }
          for (int z = 0; z < (int)Math.Pow(2, i); z++)
          {
            Console.Write("┌─");
            for (int k = 0; k < after / 2 - 1; k++)
            {
              Console.Write("─");
            }
            Console.Write("┘└");
            for (int k = 0; k < after / 2 - 1; k++)
            {
              Console.Write("─");
            }
            Console.Write("─┐");
            for (int k = 0; k < after - 1; k++)
            {
              Console.Write(" ");
            }
          }

          Console.WriteLine();
        }
      }
    }
    void SwapColors()
    {
      var color = Console.ForegroundColor;
      Console.ForegroundColor = Console.BackgroundColor;
      Console.BackgroundColor = color;
    }

    public void PrintTreeSim()
    {
      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Симметричный обход:");

      PrintTreeSim(0, 0, tree);

      Console.WriteLine("\n──────────────────────────────");
    }
    void PrintTreeSim(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      if (lvl != LastLvl)
      {
        if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
        {
          PrintTreeSim(leftChild, level, leaf);
        }
      }
      Console.Write(leaf[lvl][elem]); Console.Write(" ");

      if (lvl != LastLvl)
      {
        if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
        {
          PrintTreeSim(rightChild, level, leaf);
        }
      }
    }

    public void PrintTreeRev()
    {
      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Обратный обход:");

      PrintTreeRev(0, 0, tree);

      Console.WriteLine("\n──────────────────────────────");
    }
    void PrintTreeRev(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      if ((lvl) != LastLvl)
      {
        if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
        {
          PrintTreeRev(leftChild, level, leaf);
        }
        if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
        {
          PrintTreeRev(rightChild, level, leaf);
        }
      }
      Console.Write(leaf[lvl][elem]); Console.Write(" ");
    }

    public void PrintTreeStr()
    {
      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Прямой обход:");

      PrintTreeStr(0, 0, tree);

      Console.WriteLine("\n──────────────────────────────");
    }
    void PrintTreeStr(int elem, int lvl, List<int[]> leaf)
    {
      int leftChild = ((elem + 1) * 2) - 2;
      int rightChild = ((elem + 1) * 2) - 1;
      int level = lvl + 1;

      Console.Write(leaf[lvl][elem]); Console.Write(" ");
      if ((lvl) != LastLvl)
      {
        if (leaf[level][leftChild] != 0 && leaf[level][leftChild] != -1)
        {
          PrintTreeStr(leftChild, level, leaf);
        }
        if (leaf[level][rightChild] != 0 && leaf[level][rightChild] != -1)
        {
          PrintTreeStr(rightChild, level, leaf);
        }
      }
    }
  }
  class Program
  {
    static void Main()
    {
      Console.SetWindowSize(150, 30);
      Random rnd = new Random();
      BTree treeA = new BTree();
      BTree treeB = new BTree();
      BTree treeA_NoRandom = new BTree();
      BTree treeB_NoRandom = new BTree();
      int countInArray = 7;
      int[] array = new int[countInArray];

      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Входные числав в дерево A:");

      for (int k = 0; k < countInArray; k++)
      {
        int randomDigit = rnd.Next(1, 99);
        array[k] = randomDigit;
        Console.Write(randomDigit + " ");
      }

      Console.WriteLine("\n──────────────────────────────");

      for (int k = 0; k < countInArray; k++)
      {
        treeA_NoRandom.InsertNotRandom(array[k]);
        treeA.Insert(array[k]);
      }
      Console.WriteLine("Обычное бинарное дерево A:");
      treeA_NoRandom.PrintList();

      treeA.FixTree();

      Console.WriteLine("Рандомизированное бинарное дерево A:");
      treeA.PrintList();
      Console.WriteLine();

      treeA.PrintTreeRev();

      treeA.PrintCount();
      treeA.PrintLvl();

      treeA.PrintCountInsertAsRoot();
      treeA.PrintDataInsertAsRoot();

      Console.WriteLine("──────────────────────────────");
      Console.WriteLine("Входные числав в дерево B:");

      for (int k = 0; k < countInArray; k++)
      {
        int randomDigit = rnd.Next(1, 99);
        array[k] = randomDigit;
        Console.Write(randomDigit + " ");
      }

      Console.WriteLine("\n──────────────────────────────");

      for (int k = 0; k < countInArray; k++)
      {
        treeB_NoRandom.InsertNotRandom(array[k]);
        treeB.Insert(array[k]);
      }
      Console.WriteLine("Обычное бинарное дерево B:");
      treeB_NoRandom.PrintList();

      treeB.FixTree();

      Console.WriteLine("Рандомизированное бинарное дерево B:");
      treeB.PrintList();
      Console.WriteLine();

      treeB.PrintTreeStr();
      treeB.PrintTreeSim();

      treeB.PrintCount();
      treeB.PrintLvl();

      treeB.PrintCountInsertAsRoot();
      treeB.PrintDataInsertAsRoot();

      treeA.MergeTrees(treeA, treeB);

      treeA.FixTree();

      Console.WriteLine("Элементы дерева B добавлены в дерево A:");
      treeA.PrintList();
      Console.WriteLine();

      treeA.PrintTreeRev();

      treeA.PrintCount();
      treeA.PrintLvl();

      treeA.PrintCountInsertAsRoot();
      treeA.PrintDataInsertAsRoot();

      Console.ReadKey();
    }
  }
}
