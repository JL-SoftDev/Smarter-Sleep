import 'package:flutter/material.dart';

import 'appFrame.dart';

class AppFrameBottomAppBar extends StatefulWidget {
  const AppFrameBottomAppBar({Key? key});

  @override
  State<AppFrameBottomAppBar> createState() => _AppFrameBottomAppBarState();
}

class _AppFrameBottomAppBarState extends State<AppFrameBottomAppBar> {
  int _currentIndex = 0;

  void _onItemTapped(int index) {
    setState(() {
      _currentIndex = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return BottomAppBar(
      color: const Color(0xFF2B2B2B),
      child: Container(
        height: 56,
        padding: const EdgeInsets.symmetric(horizontal: 16),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            IconButton(
              icon: Icon(Icons.home,
                  color: _currentIndex == 0
                      ? const Color(0xFF3D5A80)
                      : const Color(0xFFA8DADC)),
              onPressed: () {
                _onItemTapped(0);
                mainNavigatorKey.currentState!.pushNamed("/home");
              },
            ),
            IconButton(
              icon: Icon(Icons.devices,
                  color: _currentIndex == 1
                      ? const Color(0xFF3D5A80)
                      : const Color(0xFFA8DADC)),
              onPressed: () {
                _onItemTapped(1);
                mainNavigatorKey.currentState!.pushNamed("/devices");
              },
            ),
            IconButton(
              icon: Icon(Icons.insert_chart,
                  color: _currentIndex == 2
                      ? const Color(0xFF3D5A80)
                      : const Color(0xFFA8DADC)),
              onPressed: () {
                _onItemTapped(2);
                mainNavigatorKey.currentState!.pushNamed("/review");
              },
            ),
          ],
        ),
      ),
    );
  }
}
