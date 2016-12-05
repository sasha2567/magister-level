package main.scala

/**
  * Created by user on 23.11.2016.
  */
object lab6 {
  class Box[T] {
    var value : T

    def save(v : T) : T = {
      value = v
    }

    def get() : T = {
      value
    }
  }

  class VBox[T] {
    def identity[T](x: T) = x

  }

  def main(args: Array[String]): Unit = {

  }
}
