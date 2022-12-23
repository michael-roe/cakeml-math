(* IEEE 754 floating point operations *)

val zero = Double.fromWord(Word64.fromInt(0));

val posInf = Double.fromWord(Word64.fromInt(0x7ff0000000000000));

val negInf = Double.fromWord(Word64.fromInt(0xfff0000000000000));

fun isZero (x) = Double.= x zero;

fun isInfinite(x) = Double.= x negInf orelse Double.= x posInf;

fun isNaN(x) = not (Double.= x x);

fun isSignMinus(x) = Word64.toIntSigned(Double.toWord(x)) < 0;

fun exponent(x) = Word64.toInt(Word64.andb(Word64.fromInt(0x7ff))(Word64.>>(Double.toWord(x))(52))) - 1023;

fun mantissa(x) = Word64.andb(Word64.fromInt(0xfffffffffffff))(Double.toWord(x));

val hexDigits = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
  "a", "b", "c", "d", "e", "f"];

fun nthDigit(x) = fn n => List.nth(hexDigits)(Word64.toInt(Word64.andb(Word64.fromInt(0xf))(Word64.>>(x)(Int.* 4 n)))); 

fun digits(x) = nthDigit(x)(12) ^ nthDigit(x)(11) ^ nthDigit(x)(10) ^
  nthDigit(x)(9) ^ nthDigit(x)(8) ^ nthDigit(x)(7) ^ nthDigit(x)(6) ^
  nthDigit(x)(5) ^ nthDigit(x)(4) ^ nthDigit(x)(3) ^ nthDigit(x)(2) ^
  nthDigit(x)(1) ^ nthDigit(x)(0);

fun nonNegativeFloatToString(x) =
  if isZero(x) then "0"
  else if isInfinite(x) then "infinity"
  else if isNaN(x) then "nan"
  else if exponent(x) = ~1023 then "0x0." ^ digits(mantissa(x)) ^ "p~1022"
  else "0x1." ^ digits(mantissa(x)) ^ "p" ^ Int.toString(exponent(x));

fun floatToString(x) =
  if isSignMinus(x) then "~" ^ nonNegativeFloatToString(Double.~ x)
  else nonNegativeFloatToString(x);
