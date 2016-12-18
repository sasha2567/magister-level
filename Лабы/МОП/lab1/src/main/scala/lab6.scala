package main.scala

/**
  * Created by user on 23.11.2016.
  */
object lab6 {
  case class Box[T](var value : T) {
    def save(v : T) = {value = v}
    def get: T = value
  }

  case class MyPrinter[T](attribute: T) {
    def print()           = println(attribute)
    def print2(arg: T)    = println(attribute + " and " + arg)
    def print3[R](arg: R) = println(attribute + " and " + arg)
  }

  class VBox[T] {
    def identity[T](x: T) = x
  }

  def main(args: Array[String]): Unit = {
    val box : Box[Int] = Box(2)
    box.save(25)
    println(box.get)
  }
}
