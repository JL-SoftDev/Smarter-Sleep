import 'package:flutter/material.dart';

void main(){
  runApp(SmarterSleep());
}
class SmarterSleep extends StatelessWidget {
  const SmarterSleep({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          backgroundColor: Color.fromARGB(255, 38, 6, 78),
          title: const Text('Smarter Sleep', selectionColor: Colors.black,)     
        ),
      ),

    );
  }
}
class SignInPage extends StatelessWidget {
  const SignInPage({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          backgroundColor: Color.fromARGB(255, 38, 6, 78),
          title: const Text('Smarter Sleep', selectionColor: Colors.black,)     
        ),
      ),

    );
  }
}
class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          backgroundColor: Color.fromARGB(255, 38, 6, 78),
          title: const Text('Smarter Sleep', selectionColor: Colors.black,)     
        ),
      ),

    );
  }
}