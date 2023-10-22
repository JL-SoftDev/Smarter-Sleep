import 'package:flutter/material.dart';

class AppFrameBottomAppBar extends StatefulWidget {
  const AppFrameBottomAppBar({super.key});

  @override
  State<AppFrameBottomAppBar> createState() => _AppFrameBottomAppBarState();
}

class _AppFrameBottomAppBarState extends State<AppFrameBottomAppBar> {
  @override
  Widget build(BuildContext context) {
    return Container(
        height: 56,
        padding: const EdgeInsets.symmetric(horizontal: 0),
        color: const Color.fromARGB(255, 99, 94, 86),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            Container(
              width: 56,
              color: Colors.orange,
            ),
            Container(
              width: 56,
              color: Colors.orange,
            ),
            Container(
              width: 56,
              color: Colors.orange,
            )
          ],
        ));
  }
}
