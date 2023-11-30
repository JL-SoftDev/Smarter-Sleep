import 'package:flutter/material.dart';

//Potentially add a mid int and color
class ColorUtils {
  static Color getScoreColor(int score,
      {int min = 0,
      int max = 100,
      Color start = Colors.red,
      Color end = Colors.green}) {
    if (score < min) {
      return start;
    } else if (score > max) {
      return end;
    } else {
      final colorTween = ColorTween(
        begin: start,
        end: end,
      );
      return colorTween.transform(score / max)!;
    }
  }
}
