import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';

import '../appFrame.dart';
import 'homeScreen.dart';
import 'inventoryScreen.dart';
import 'settingsScreen.dart';
import 'shopScreen.dart';
import 'statsScreen.dart';
import 'accountPage.dart';

class TestingScreen extends StatefulWidget {
  const TestingScreen({super.key});

  @override
  State<TestingScreen> createState() => _TestingScreenState();
}

class _TestingScreenState extends State<TestingScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Test Page"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: _navigateToAccountPage,
          ),
        ],
      ),
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
                      onPressed: _navigateToHomeScreen,
                      child: Text("Home Screen")),
                  TextButton(
                      onPressed: _navigateToInventoryScreen,
                      child: Text("Inventory")),
                  //TextButton(
                  //onPressed: _navigateToShopScreen, child: Text("Shop")),
                  //TextButton(
                  //onPressed: _navigateToStatsScreen, child: Text("Stats")),
                  TextButton(
                      onPressed: _navigateToSettingsScreen,
                      child: Text("Settings")),
                  TextButton(
                      onPressed: _navigateToDevicesScreen,
                      child: Text("Connected Devices")),
                ]))
      ]),
    );
  }

  //Basic functions to navigate screens
<<<<<<< HEAD
  void _navigateToLoginScreen() {
    mainNavigatorKey.currentState!.pushNamed("/login");
  }

=======
>>>>>>> main
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

  void _navigateToDevicesScreen() {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => const DeviceConnectionsScreen()));
  }

  void _navigateToAccountPage() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => const AccountPage()),
    );
  }
}
