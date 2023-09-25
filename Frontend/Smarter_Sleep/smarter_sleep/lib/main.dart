import 'package:flutter/material.dart';
import 'app/screens/testingScreen.dart';

void main() {
  runApp(const MaterialApp(title: "Smarter Sleep", home: TestingScreen()));
}
/*
class SmarterSleep extends StatelessWidget {
  const SmarterSleep({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
            backgroundColor: Color.fromARGB(255, 217, 185, 255),
            title: const Text('Smarter Sleep',
                style: TextStyle(color: Color.fromARGB(255, 90, 68, 255)))),
      ),
    );
  }
}
*/
