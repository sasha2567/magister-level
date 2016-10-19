package main.scala

/**
  * Created by user on 18.10.2016.
  */
object TreeOption {

    def printTree(tree: Tree): Unit = {
      if(tree != null) {
        printTree(tree.getLeftSubtree)
        println(tree.getNodeData)
        printTree(tree.getRightSubtree)
      }
    }

    def insert(nodeData: Int, tree: Tree): Tree = {
      if(tree == null) Leaf(nodeData)
      else {
        if (nodeData <= tree.getNodeData) Node(insert(nodeData, tree.getLeftSubtree), tree.getRightSubtree, tree.getNodeData)
        else Node(tree.getLeftSubtree, insert(nodeData, tree.getRightSubtree), tree.getNodeData)
      }
    }

    def contains(nodeData: Int, tree: Tree): Boolean = {
      if(tree == null) false
      else {
        if(nodeData == tree.getNodeData) true
        else {
          if(nodeData < tree.getNodeData) contains(nodeData, tree.getLeftSubtree)
          else contains(nodeData, tree.getRightSubtree)
        }
      }
    }

    def sum(tree: Tree): Long = {
      if(tree == null) 0
      else sum(tree.getLeftSubtree) + sum(tree.getRightSubtree) + tree.getNodeData
    }
}
