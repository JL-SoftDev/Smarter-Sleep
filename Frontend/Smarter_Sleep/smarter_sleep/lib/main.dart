import 'package:flutter/material.dart';

void main() {
  runApp(HomeScreen());
}

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

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: new ThemeData(
          scaffoldBackgroundColor: Color.fromARGB(255, 246, 246, 250)),
      home: Scaffold(
        floatingActionButtonLocation: FloatingActionButtonLocation.endTop,
        floatingActionButton: SizedBox(
          height: 50,
          width: 50,
          child: FloatingActionButton(
            shape: const BeveledRectangleBorder(borderRadius: BorderRadius.zero),
            onPressed: () {
              print("This is a Sleep Button");
            },
            child: const Icon(Icons.timer),
          ),
        ),
        appBar: AppBar(
            backgroundColor: Color.fromARGB(255, 36, 22, 65),
            leading: const Icon(Icons.circle, color: Colors.amber),
            leadingWidth: 40,
            title: const Text('Coin Counter'),
            actions: [const Icon(Icons.settings)]),
        bottomNavigationBar: BottomNavigationBar(
            backgroundColor: Color.fromARGB(255, 39, 22, 74),
            items: const [
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Home'),
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Shop'),
              BottomNavigationBarItem(icon: Icon(Icons.square), label: 'Stats')
            ]),
      ),
    );
  }
}
