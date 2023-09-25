import 'package:flutter/material.dart';

import 'homeScreen.dart';
import 'loginScreen.dart';
import 'settingsScreen.dart';

class TestingScreen extends StatefulWidget {
  const TestingScreen({super.key});

  @override
  State<TestingScreen> createState() => _TestingScreenState();
}

class _TestingScreenState extends State<TestingScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(children: [
        //Just placeholder text
        Flexible(
            child:
                Center(child: Text("Smarter Sleep Android/iOS Application"))),
        //Spacer
        //TESTING Bottom Links to pages
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            TextButton(
                onPressed: _navigateToLoginScreen, child: Text("Login Screen")),
            TextButton(
                onPressed: _navigateToHomeScreen, child: Text("Home Screen")),
            TextButton(
                onPressed: _navigateToSettingsScreen, child: Text("Settings")),
          ],
        )
      ]),
    );
  }

  //Basic functions to navigate screens
  void _navigateToLoginScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const LoginScreen()));
  }

  void _navigateToHomeScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const HomeScreen()));
  }

  void _navigateToSettingsScreen() {
    Navigator.push(context,
        MaterialPageRoute(builder: (context) => const SettingsScreen()));
  }
}
