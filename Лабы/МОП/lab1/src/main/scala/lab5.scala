package main.scala

import scala.util.Random

/**
  * Created by user on 13.11.2016.
  */
object lab5 {

  def counterBuilder(): () => Int = {
    var count: Int = 0
    def callCounter(): Int = {
      count += 1
      count
    }
    callCounter
  }

  def десятичноеЧислоВNичное(c: Int, n: Int) : String = {
    def numToStr(i: Int) : String = {
      val s = i match {
        case 10 => "A"
        case 11 => "B"
        case 12 => "C"
        case 13 => "D"
        case 14 => "E"
        case 15 => "F"
        case _  => i+""
      }
      s
    }
    def numberToNumber2(num: Int, osn: Int, s: String) : String = {
      var str = s
      if(osn > num) {
        str = numToStr(num) + str
        str
      } else {
        str = numToStr(num % osn) + str
        numberToNumber2(num/osn, osn, str)
      }
    }
    numberToNumber2(c, n, "")
  }

  def listBuilder(z: Int)(length: Int): List[Int] = {
    val random: Random = Random
    def listBuilder(length: Int, acc: List[Int]): List[Int] = {
      if(length == 0) acc
      else {
        val element = z + (random.nextInt() % 6)
        listBuilder(length - 1, element :: acc)
      }
    }
    listBuilder(length, Nil)
  }


  def main(args: Array[String]): Unit = {
    val callCounter = counterBuilder()
    println(callCounter())
    println(callCounter())
    val func = десятичноеЧислоВNичное(_: Int,2)
    println(func(18))
    val func1 = десятичноеЧислоВNичное(_: Int,16)
    println(func1(31))
    val listBuilderWith1Medium = listBuilder(1)(_)
    println(listBuilderWith1Medium(10))

  }
}
