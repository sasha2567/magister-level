package main.scala

/**
  * Created by user on 18.10.2016.
  */
trait Tree {
  def getLeftSubtree: Tree
  def getRightSubtree: Tree
  def getNodeData: Int
}
