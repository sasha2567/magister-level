package main.scala

/**
  * Created by user on 25.10.2016.
  */
object lab4 {
  //abstract class Pair
  //case class ZeroElement(dataFirst: Int, dataSecond: Int) extends Pair
  case class Element(dataFirst: Int, dataSecond: Int)// extends Pair

  abstract class NodeElement
  case class Operation(data: Int) extends NodeElement
  case class Variable(data: Char) extends NodeElement

  abstract class Tree
  case class Leaf(data: NodeElement) extends Tree
  case class Node(data: NodeElement, left: Tree, right: Tree) extends Tree

  def printElement(el: NodeElement): Any = {
    el match {
      case Operation(o) => o match {
        case 1 => '+'
        case 2 => '-'
        case 3 => '*'
        case 4 => '/'
      }
      case Variable(v) => v
    }
  }

  def printTree(tree: Tree, lvl: Int): Unit = {
    tree match {
      case Node(d, l, r) =>
        printTree(l, lvl + 1)
        println(" " * lvl + printElement(d))
        printTree(r, lvl + 1)
      case Leaf(d) => println(" " * lvl + printElement(d))
    }
  }

  def printTreeElement(tree: Tree): Unit = {
    tree match {
      case Node(d, l, r) =>
        printTreeElement(l)
        printTreeElement(r)
        print(printElement(d))
      case Leaf(d) => print(printElement(d))
    }
  }


  def listDivisionWithCase(lst: List[(Element)]) : List[Option[Double]] = {
    if (lst == Nil)
      return Nil
    val res = lst.head match {
      case Element(dF, 0)=> None
      case Element(dF, dS) => Some(dF * 1.0/ dS)
    }
    val result = res :: listDivisionWithCase(lst.tail)
    result
  }

  def listDivision(lst: List[(Int,Int)]) : List[Option[Double]] = {
    if (lst == Nil)
      return Nil
    val res = lst.head._2 match {
      case 0 => None
      case _ => Some(lst.head._1 * 1.0/ lst.head._2)
    }
    val result = res :: listDivision(lst.tail)
    result
  }

  def printDivision(lst:List[Option[Double]]) : List[String] = {
    if (lst == Nil)
      return Nil
    val res = lst.head match {
      case Some(double) => "Результат деления = "+double
      case None => "Деление на ноль невозможно"
    }
    val result = res :: printDivision(lst.tail)
    result
  }

  def main(args: Array[String]): Unit = {
    val list = List((1,2),(2,3),(1,0))
    println(printDivision(listDivision(list)))
    val pairList: List[Element] = List(Element(1,2), Element (2,3), Element(1,0))
    println(printDivision(listDivisionWithCase(pairList)))

    println("\n----------------\n")

    val tree = Node(
      Operation(1),
      Node(
        Operation(3),
        Leaf(
          Variable('a')
        ),
        Leaf(
          Variable('b')
        )
      ),
      Node(
        Operation(4),
        Node(
          Operation(2),
          Leaf(
            Variable('c')
          ),
          Leaf(
            Variable('d')
          )
        ),
        Leaf(
          Variable('e')
        )
      )
    )
    printTree(tree,1)
    println()
    printTreeElement(tree)
  }
}
