import 'package:flutter/material.dart';

class SignInPage extends StatelessWidget {
  const SignInPage({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
            backgroundColor: Color.fromARGB(255, 38, 6, 78),
            title: const Text('Home',
                style: TextStyle(color: Color.fromARGB(255, 90, 68, 255)),
                textAlign: TextAlign.center)),
      ),
    );
  }
}