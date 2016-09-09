#!/usr/bin/perl

use Time::UTC;
use strict;

# генерим фейковые данные для вывода на графике

# print sin(3.14);

print "all_data = [\n";
for my $data (1..5) {
  my $x = rand()*10;
  print "  [";
  for my $year (2006..2010) {
    for my $month (1..12) {
      print "[\"$year/".sprintf("%02d", $month)."/01\", ".int(20 + sin($x)*20)."], ";
      $x += 0.50;
    }
  }
  print "],\n";
}
print "];\n";
