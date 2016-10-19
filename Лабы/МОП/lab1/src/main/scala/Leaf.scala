package main.scala

/**
  * Created by user on 18.10.2016.
  */
class Leaf extends Tree {

  private var data: Int = 0

  def this(nodeData: Int) {
    this()
    this.data = nodeData
  }

  def getLeftSubtree: Tree = null
  def getRightSubtree: Tree = null
  def getNodeData: Int = data
}

object Leaf {

  def apply: Leaf = new Leaf()
  def apply(leafData:Int): Leaf = new Leaf(leafData)

}
