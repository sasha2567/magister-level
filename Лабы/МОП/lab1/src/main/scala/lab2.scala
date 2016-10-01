package main.scala

/**
  * Created by user on 28.09.2016.
  */
object lab2 {

  /*def processList(lst: List[Int], func: (Int) => Int): List[Int] = {
    if (lst == Nil) Nil  else func(lst.head) :: processList(lst.tail, func)
  }*/

  def main (args: Array[String]): Unit = {
    def processList(lst: List[Int], func: (Int) => Int): List[Int] = {
      def processListRec(_list: List[Int],_func: (Int)=> Int, acc: List[Int]): List[Int]={
        if (_list == Nil) acc
        else processListRec(_list.tail,_func,_func(_list.head)::acc)
      }
      processListRec(lst,func,Nil).reverse
    }

    def transformIntList(lst:List[Int]): List[String] =
    {
      (for (i <- 0 until lst.length) yield {
        s"Элемент под номером $i равен "+lst(i)
      }).toList
    }

    def getAVTYear(lst: List[Student]): List[Int]={
      val studentsList = lst.filter(_._4 == "AVT").filter(_._7 == true)
      for (i<-studentsList) yield (i._3)
    }

    def getHostelNeghbors(lst: List[Student], rm: List[Room]): List[_]={//(String, Int, Int)] = {
      var pairRoomHostel = rm.zip(rm.tail)
      var avtStudnts = lst.filter(_._4 == "AVT").filter(_._7 == true)
      var students = for (i <- avtStudnts) yield (i._1)
      var roomNeghbors = pairRoomHostel.filter(r=>(r._1._1 -r._2._1).abs == 1)
      //roomNeghbors
      pairRoomHostel.filter(r=>r._1._3.contains(students))
    }

    type Student = (
      Int,     // ID
      String,  // Имя
      Int,     // Год рождения
      String,  // Факультет
      Char,    // Пол
      Int,     // Курс
      Boolean  // Проживает ли в общежитии
    )

    val students: List[Student] = List(
      (0, "Алёна", 1995, "FIL", 'F', 1, true),
      (1, "Гриша", 1994, "AVT", 'M', 2, true),
      (2, "Настя", 1993, "MTS", 'F', 3, false),
      (3, "Коля", 1997, "MTS", 'M', 1, false),
      (4, "Миша", 1997, "AVT", 'M', 3, true),
      (5, "Оля", 1992, "FIL", 'F', 3, false),
      (6, "Маша", 1991, "AVT", 'F', 5, true),
      (7, "Таня", 1993, "FIL", 'M', 4, true),
      (8, "Женя", 1992, "FIL", 'F', 4, true),
      (9, "Света", 1989, "AVT", 'F', 3, true),
      (10, "Аня", 1996, "MTS", 'F', 4, false),
      (11, "Лена", 1996, "AVT", 'F', 2, true),
      (12, "Сергей", 1994, "FIL", 'M', 3, false),
      (13, "Влад", 1993, "FIL", 'M', 5, false),
      (14, "Гена", 1996, "MTS", 'M', 1, true),
      (15, "Дима", 1995, "AVT", 'M', 5, false),
      (16, "Катя", 1991, "FIL", 'F', 4, false),
      (17, "Артём", 1994, "MTS", 'M', 3, true),
      (18, "Диана", 1995, "FIL", 'M', 4, false)
    )

    type Room = (
      Int,       // Номер комнаты
      Int,       // Вместимость комнаты
      List[Int]  // ID студентов, проживающих в комнате
    )

    val rooms: List[Room] = List(
      (37, 3, List(0, 7, 8)),
      (42, 2, List(1, 4)),
      (43, 3, List(6, 9, 11)),
      (54, 2, List(14, 17))
    )

    val list = List.range(1, 6)
    println("Source list: "+list)
    println("Source list +2:"+processList(list,_ * 5)+"\n")
    println("Source list: "+list)
    println(transformIntList(list)+"\n")
    println("AVT year in hostel: "+getAVTYear(students))
    println(getHostelNeghbors(students,rooms))
  }
}
