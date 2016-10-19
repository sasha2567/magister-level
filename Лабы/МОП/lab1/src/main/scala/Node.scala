package main.scala

/**
  * Created by user on 18.10.2016.
  */
class Node extends Tree{
  private var left : Tree = null
  private var right : Tree = null
  private var data : Int = 0

  def getLeftSubtree: Tree = left
  def getRightSubtree: Tree = right
  def getNodeData: Int = data

  def this(nodeData: Int) {
    this()
    this.data = nodeData
  }

  def this(leftSubtree: Tree, rightSubtree: Tree, nodeData: Int) {
    this(nodeData)
    this.left = leftSubtree
    this.right = rightSubtree
  }
}

object Node {

  def apply: Node = new Node()
  def apply(nodeData: Int): Node = new Node(nodeData)
  def apply(leftSubtree: Tree, rightSubtree: Tree, nodeData: Int): Node = new Node(leftSubtree, rightSubtree, nodeData)

}
