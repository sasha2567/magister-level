package main.scala


/**
  * Created by user on 18.09.2016.
  */
object Application {
  def createAbsList(in: Int): List[List[Int]] = {
    List(List.range(0, Math.abs(in) + 1))
  }

  def moduleList(lst: List[Int]): List[Any] = {
    def moduleListAcc(__list: List[Int], acc: List[Any]): List[Any] = {
      if (__list != Nil) {
        moduleListAcc(__list.tail, acc ::: createAbsList(__list.head))
      }
      else
        acc
    }
    moduleListAcc(lst, Nil)
  }

  def moduleListRec(lst: List[Int]): List[Any] = {
    if (lst == Nil)
      Nil
    else
      createAbsList(lst.head) ::: moduleListRec(lst.tail)
  }

  def main(args: Array[String]): Unit = {
    var list = List.range(0,6500,2);
    println(moduleList(list))
    println(moduleListRec(list))
  }
}
