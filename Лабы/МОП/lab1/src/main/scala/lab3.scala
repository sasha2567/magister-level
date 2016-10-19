package main.scala

/**
  * Created by user on 18.10.2016.
  */
object lab3 {

  def main (args: Array[String]): Unit = {
    var tree: Tree = Leaf(3)
    tree = TreeOption.insert(23, tree)
    tree = TreeOption.insert(72, tree)
    tree = TreeOption.insert(5, tree)
    tree = TreeOption.insert(92, tree)
    tree = TreeOption.insert(261, tree)
    tree = TreeOption.insert(0, tree)
    tree = TreeOption.insert(12, tree)
    tree = TreeOption.insert(303, tree)
    tree = TreeOption.insert(922, tree)
    TreeOption.printTree(tree)
    println(TreeOption.contains(261, tree))
    println(TreeOption.contains(1, tree))
    println(TreeOption.sum(tree))
    val tree1 : Tree = TreeOption.insert(1, tree)
    println(TreeOption.contains(1, tree1))
    println("----------------------")
    TreeOption.printTree(tree)
    println("---------Tree--------")
    TreeOption.printTree(tree1)
  }
}
