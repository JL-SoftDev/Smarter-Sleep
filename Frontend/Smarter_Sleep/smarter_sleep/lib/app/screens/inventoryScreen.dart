import 'package:flutter/material.dart';

import '../appFrame.dart';

class InventoryScreen extends StatefulWidget {
  const InventoryScreen({super.key});

  @override
  State<InventoryScreen> createState() => _InventoryScreenState();
}

class _InventoryScreenState extends State<InventoryScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("Inventory"),
          actions: [
            IconButton(
              icon: const Icon(Icons.account_circle),
              onPressed: () {
                mainNavigatorKey.currentState!.pushNamed("/account");
              },
            ),
          ],
        ),
        body: Column(children: [
          //Top Section
          Container(
            height: MediaQuery.of(context).size.height * 0.3,
            width: double.infinity,
            padding: EdgeInsets.symmetric(horizontal: 32),
            color: Colors.grey.shade700,
            child: Row(
              children: [
                Container(
                  color: Colors.grey.shade300,
                  height: MediaQuery.of(context).size.height * 0.15,
                  width: MediaQuery.of(context).size.height * 0.15,
                ),
                Spacer(),
                Container(
                  color: Colors.grey.shade300,
                  height: MediaQuery.of(context).size.height * 0.15,
                  width: MediaQuery.of(context).size.height * 0.15,
                ),
              ],
            ),
          ),
          //Bottom Section
          Expanded(
              child: Container(
            color: Colors.grey,
            width: double.infinity,
            child:
                Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              Text("TODO - Add a grid"),
            ]),
          ))
        ]));
  }
}
