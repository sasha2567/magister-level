package main.scala

/**
  * Created by user on 25.10.2016.
  */
object lab4 {
  abstract class Pair
  case class ZeroElement(dataFirst: Int, dataSecond: Int) extends Pair
  case class Element(dataFirst: Int, dataSecond: Int) extends Pair

  def listDivisionWithCase(lst: List[(Pair)]) : List[Option[Double]] = {
    if (lst == Nil)
      return Nil
    val res = lst.head match {
      case Element(dF, dS) => Some(dF * 1.0/ dS)
      case ZeroElement(dF, dS)=> None
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
    val pairList: List[Pair] = List(new Element(1,2),new Element (2,3), new ZeroElement(1,0))
    println(printDivision(listDivisionWithCase(pairList)))
  }
}
