import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';

import '../appFrame.dart';
import 'homeScreen.dart';
import 'inventoryScreen.dart';
import 'loginScreen.dart';
import 'settingsScreen.dart';
import 'shopScreen.dart';
import 'statsScreen.dart';

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
        Spacer(),
        //TESTING Bottom Links to pages
        Container(
            height: 48,
            child: ListView(
                //mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                scrollDirection: Axis.horizontal,
                children: [
                  TextButton(
                      onPressed: _navigateToLoginScreen,
                      child: Text("Login Screen")),
                  TextButton(
                      onPressed: _navigateToHomeScreen,
                      child: Text("Home Screen")),
                  TextButton(
                      onPressed: _navigateToInventoryScreen,
                      child: Text("Inventory")),
                  TextButton(
                      onPressed: _navigateToShopScreen, child: Text("Shop")),
                  TextButton(
                      onPressed: _navigateToStatsScreen, child: Text("Stats")),
                  TextButton(
                      onPressed: _navigateToSettingsScreen,
                      child: Text("Settings")),
                ]))
      ]),
    );
  }

  //Basic functions to navigate screens
  void _navigateToLoginScreen() {
    mainNavigatorKey.currentState!.pushNamed("/login");
  }

  void _navigateToHomeScreen() {
    mainNavigatorKey.currentState!.pushNamed("/home");
  }

  void _navigateToInventoryScreen() {
    mainNavigatorKey.currentState!.pushNamed("/inventory");
  }

  void _navigateToShopScreen() {
    mainNavigatorKey.currentState!.pushNamed("/shop");
  }

  void _navigateToStatsScreen() {
    mainNavigatorKey.currentState!.pushNamed("/stats");
  }

  void _navigateToSettingsScreen() {
    mainNavigatorKey.currentState!.pushNamed("/settings");
  }
}
