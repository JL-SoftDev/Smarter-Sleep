import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/pageRouter.dart';
import 'appFramebottomAppBar.dart';

//The app frame acts as a universal widget that is overlayed over ALL pages,
// navigation is down within the app frame to keep consistency of UI across all
// pages, there may be a better way of doing this, just emulating what I have done
// in my prior applicaiton - Jozef

class AppFrame extends StatefulWidget {
  const AppFrame({super.key});

  @override
  State<AppFrame> createState() => _AppFrameState();
}

final GlobalKey<NavigatorState> mainNavigatorKey = GlobalKey<NavigatorState>();

class _AppFrameState extends State<AppFrame> {
  String initialRoute = "/";

  @override
  void initState() {
    initialRoute = "/home";
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: _onWillPop,
      child: Scaffold(
          body: Navigator(
              key: mainNavigatorKey,
              initialRoute: initialRoute,
              onGenerateRoute: _onGenerateRoute),
          bottomNavigationBar: const AppFrameBottomAppBar()),
    );
  }

  ///This function is simply so that the user cannot back from the last widget
  ///and close the application, this will allow our inner (Main Navigation)
  ///to be able to be popped as needed
  Future<bool> _onWillPop() async {
    //Handle popping of inner (main) navigation
    if (mainNavigatorKey.currentState!.canPop()) {
      mainNavigatorKey.currentState!.pop(context);
      return false;
    }
    //Prevent popping of main navigation stack, preventing closure of application
    return false;
  }

  //Handles route generation, larger function is contained within class to save space
  Route<dynamic>? _onGenerateRoute(RouteSettings settings) {
    return PageRouter.getRouteBuilder(settings);
  }
}
