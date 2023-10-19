import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';

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
  void _navigateToLoginScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const LoginScreen()));
  }

  void _navigateToHomeScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const HomeScreen()));
  }

  void _navigateToInventoryScreen() {
    Navigator.push(context,
        MaterialPageRoute(builder: (context) => const InventoryScreen()));
  }

  void _navigateToShopScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const ShopScreen()));
  }

  void _navigateToStatsScreen() {
    Navigator.push(
        context, MaterialPageRoute(builder: (context) => const StatsScreen()));
  }

  void _navigateToSettingsScreen() {
    Navigator.push(context,
        MaterialPageRoute(builder: (context) => const SettingsScreen()));
  }
  void _navigateToDevicesScreen() {
    Navigator.push(context,
        MaterialPageRoute(builder: (context) => const DeviceConnectionsScreen()));
  }
}
