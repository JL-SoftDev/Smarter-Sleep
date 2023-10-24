import 'package:flutter/material.dart';

class LoginInputWidget extends StatefulWidget {
  final TextEditingController controller;
  final String label;
  final String hintText;
  final bool obscureText;
  const LoginInputWidget(
      {super.key,
      required this.controller,
      required this.label,
      required this.hintText,
      this.obscureText = false});

  @override
  State<LoginInputWidget> createState() => _LoginInputWidgetState();
}

class _LoginInputWidgetState extends State<LoginInputWidget> {
  @override
  Widget build(BuildContext context) {
    return Flexible(
        child: Column(children: [
      Text(widget.label),
      Flexible(
          child: TextField(
              controller: widget.controller,
              obscureText: widget.obscureText,
              decoration: InputDecoration(
                  border: const OutlineInputBorder(), hintText: widget.hintText)))
    ]));
  }
}
