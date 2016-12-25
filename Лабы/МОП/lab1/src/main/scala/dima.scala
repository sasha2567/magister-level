package main.scala

import main.scala.lab6.Function

object dima {
  trait covBox[T] extends Function[T]{
    val value=null.asInstanceOf[T]

    val get:T
    val save = value;

    def map[R](f:T=>R): covBox[R]
  }
  def main(args: Array[String]): Unit = {

  }
}
