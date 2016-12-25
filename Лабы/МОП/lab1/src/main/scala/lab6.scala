package main.scala

/**
  * Created by user on 23.11.2016.
  */
object lab6 {
  trait Function[T] {
    def get: T
  }

  trait CovarFunctor[T] {
    def map[R](f: T => R): CovarFunctor[R]
  }

  trait CFunctor[T] extends Function[T] with CovarFunctor[T] { outer =>
    def source: CFunctor[_] = this
    def map[R](f: T => R) = new CFunctor[R] {
      override def source = outer
      def get = f(source.get)
    }
  }

  object CFunctor {
    def apply[T](value: T) = new CFunctor[T] {
      def get = value
    }
  }

  trait Set[T] {
    def set: T => Unit
  }

  trait ContraFunctor[T] {
    def contramap[R](f: R => T): ContraFunctor[R]
  }

  trait ConFunctor[T] extends Set[T] with ContraFunctor[T] { outer =>
    def storage: ConFunctor[_] = this
    def contramap[R](f: R => T) = new ConFunctor[R] {
      override def storage = outer
      override def set = (newValue: R) => storage.set(f(newValue))
    }
  }

  object ConFunctor {
    def apply[T] = new ConFunctor[T] {
      def set = (newValue: T) => Unit
    }
  }

  trait InvariantFunctor[T] {
    def xmap[R](in: R => T, out: T => R): InvariantFunctor[R]
  }

  trait Box[T] extends Function[T] with Set[T] with InvariantFunctor[T] { outer =>
    def xmap[R](in: R => T, out: T => R) = new Box[R] {
      def get = out(outer.get)
      def set = (newValue: R) => outer.set(in(newValue))
    }
  }

  object Box {
    def apply[T](value: T): Box[T] = new Box[T] {
      var storage = value
      def get = storage
      def set = (newValue: T) => storage = newValue
    }
  }

  def main(args: Array[String]): Unit = {
    val box : Box[Int] = Box(2)
    println(box.get == box.xmap[Int](x => x, x => x).get)
    val cBox : ConFunctor[Int] = ConFunctor[Int]
    cBox.set(3)
    println(cBox.storage == cBox.contramap[Int](x => x).storage)
    val conBox : CFunctor[Int] = CFunctor(4)
    println(conBox.get == conBox.map[Int](x => x).get)
  }
}
