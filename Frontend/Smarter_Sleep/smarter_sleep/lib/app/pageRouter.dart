//Page Router class serves as a to simple return a builder object when provided
//  a route name
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/screens/debugScreen.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';
import 'package:smarter_sleep/app/screens/homeScreen.dart';
import 'package:smarter_sleep/app/screens/accountScreen.dart';
import 'package:smarter_sleep/app/screens/reviewScreen.dart';

class PageRouter {
  static MaterialPageRoute getRouteBuilder(RouteSettings settings) {
    late WidgetBuilder builder;
    switch (settings.name) {
      //Default Screen / Login Screen
      case '/':
        builder = (BuildContext _) => const HomeScreen();
        break;

      //Main Screens
      case '/home':
        builder = (BuildContext _) => const HomeScreen();
        break;
      case '/devices':
        builder = (BuildContext _) => const DeviceConnectionsScreen();
        break;
      case '/review':
        builder = (BuildContext _) => const SleepReviewScreen();
        break;
      case '/account':
        builder = (BuildContext _) => const AccountScreen();
        break;

      //DEBUG SCREENS
      case '/debug':
        builder = (BuildContext _) => const DebugScreen();
        break;

      default:
        throw Exception('Invalid route: ${settings.name}');
    }
    return MaterialPageRoute(builder: builder, settings: settings);
  }
}
