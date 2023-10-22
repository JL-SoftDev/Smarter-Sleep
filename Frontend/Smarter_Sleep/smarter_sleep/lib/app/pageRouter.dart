//Page Router class serves as a to simple return a builder object when provided
//  a route name
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';
import 'package:smarter_sleep/app/screens/homeScreen.dart';
import 'package:smarter_sleep/app/screens/inventoryScreen.dart';
import 'package:smarter_sleep/app/screens/loginScreen.dart';
import 'package:smarter_sleep/app/screens/settingsScreen.dart';
import 'package:smarter_sleep/app/screens/shopScreen.dart';
import 'package:smarter_sleep/app/screens/statsScreen.dart';

import 'screens/testingScreen.dart';

class PageRouter {
  static MaterialPageRoute getRouteBuilder(RouteSettings settings) {
    late WidgetBuilder builder;
    switch (settings.name) {
      //Default Screen / Login Screen
      case '/':
        builder = (BuildContext _) => const TestingScreen();
        break;

      //Main Screens
      case '/home':
        builder = (BuildContext _) => const HomeScreen();
        break;
      case '/devices':
        builder = (BuildContext _) => const DeviceConnectionsScreen();
        break;
      case '/inventory':
        builder = (BuildContext _) => const InventoryScreen();
        break;
      case '/shop':
        builder = (BuildContext _) => const ShopScreen();
        break;
      case '/stats':
        builder = (BuildContext _) => const StatsScreen();
        break;
      case '/settings':
        builder = (BuildContext _) => const SettingsScreen();
        break;

      //Login Screens
      case '/login':
        builder = (BuildContext _) => const LoginScreen();
        break;
      //DEBUG SCREENS

      default:
        throw Exception('Invalid route: ${settings.name}');
    }
    return MaterialPageRoute(builder: builder, settings: settings);
  }
}
