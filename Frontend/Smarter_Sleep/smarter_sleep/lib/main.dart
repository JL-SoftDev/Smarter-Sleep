import 'package:flutter/material.dart';
import 'HomeScreen.dart';
//import 'SignInPage.dart';

void main() {
  runApp(SmarterSleep());
}

class SmarterSleep extends StatelessWidget {
  const SmarterSleep({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: HomeScreen(),
    );
  }
}
