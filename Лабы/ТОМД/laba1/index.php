<?php

function Ak($k)
{
    return 8 * ( sin( 0.5 * $k * pi() )  - sin($k * pi() ) ) / ( $k * pi() * 2 );
}

$segmentOne = -(-0.5 - (-1));
$segmentTwo = 0.5 - (-0.5);
$segmentThree = -(1 - 0.5);

$firstA = $segmentOne + $segmentTwo + $segmentThree;

$file = fopen('result.csv', 'w');

for ( $j = -1; $j <= 1; $j += 0.001 )
{
    $result = $firstA;
    for ( $i = 1; $i <= 100; $i++ )
    {
        $result += Ak($i) * cos( $i * pi() * $j );
    }
    if ($j <= -0.5) {
    	$standart = -1;	
    } elseif ($j <= 0.5) {
    	$standart = 1;
    } else {
    	$standart = -1;
    }
    fputcsv( $file, [ str_replace( '.', ',', $j ), str_replace( '.', ',', $result ), $standart ], ';' );
}
fclose($file);