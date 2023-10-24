import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';

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
        const Flexible(
            child:
                Center(child: Text("Smarter Sleep Android/iOS Application"))),
        const Spacer(),
        //TESTING Bottom Links to pages
        SizedBox(
            height: 48,
            child: ListView(
                //mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                scrollDirection: Axis.horizontal,
                children: [
                  TextButton(
                      onPressed: _navigateToHomeScreen,
                      child: const Text("Home Screen")),
                  TextButton(
                      onPressed: _navigateToInventoryScreen,
                      child: const Text("Inventory")),
                  //TextButton(
                  //onPressed: _navigateToShopScreen, child: Text("Shop")),
                  //TextButton(
                  //onPressed: _navigateToStatsScreen, child: Text("Stats")),
                  TextButton(
                      onPressed: _navigateToSettingsScreen,
                      child: const Text("Settings")),
                  TextButton(
                      onPressed: _navigateToDevicesScreen,
                      child: const Text("Connected Devices")),
                ]))
      ]),
    );
  }

  //Basic functions to navigate screens
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
