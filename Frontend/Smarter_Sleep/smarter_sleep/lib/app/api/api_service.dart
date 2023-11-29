import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:smarter_sleep/app/appFrame.dart';

class ApiService {
  static const bool debug = true;

  static const String baseUrl = !debug
      ? "http://ec2-54-87-139-255.compute-1.amazonaws.com"
      : "http://10.0.2.2";

  static Future<dynamic> get(String endpoint) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/$endpoint'));
      return _parseResponse(response);
    } on HttpStatusException catch (e) {
      _showErrorDialog(statusCode: e.statusCode);
      return null;
    } catch (error) {
      _showErrorDialog();
      return null;
    }
  }

  static Future<dynamic> post(String endpoint, dynamic body) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/$endpoint'),
        body: jsonEncode(body),
        headers: {'Content-Type': 'application/json'},
      );
      return _parseResponse(response);
    } on HttpStatusException catch (e) {
      _showErrorDialog(statusCode: e.statusCode);
      return null;
    } catch (error) {
      _showErrorDialog();
      return null;
    }
  }

  static Future<dynamic> put(String endpoint, dynamic body) async {
    try {
      final response = await http.put(
        Uri.parse('$baseUrl/$endpoint'),
        body: jsonEncode(body),
        headers: {'Content-Type': 'application/json'},
      );
      return _parseResponse(response);
    } on HttpStatusException catch (e) {
      _showErrorDialog(statusCode: e.statusCode);
      return null;
    } catch (error) {
      _showErrorDialog();
      return null;
    }
  }

  static Future<dynamic> delete(String endpoint) async {
    try {
      final response = await http.delete(Uri.parse('$baseUrl/$endpoint'));
      return _parseResponse(response);
    } on HttpStatusException catch (e) {
      _showErrorDialog(statusCode: e.statusCode);
      return null;
    } catch (error) {
      _showErrorDialog();
      return null;
    }
  }

  static dynamic _parseResponse(http.Response response) {
    if (response.statusCode >= 200 && response.statusCode < 300) {
      return jsonDecode(response.body);
    } else {
      throw HttpStatusException(response.statusCode);
    }
  }

  static void _showErrorDialog({int statusCode = -1}) {
    String title = 'Error';
    String description =
        'An unknown client error has occured, unable to verify request.';

    if (statusCode != -1) {
      title = 'Error Code: $statusCode';
      description = 'Sorry, the server was unable to fulfill your request.';
    }

    showDialog(
      context: mainNavigatorKey.currentContext!,
      builder: (context) => AlertDialog(
        title: Text(title),
        content: Text(description),
        actions: <Widget>[
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('OK'),
          ),
        ],
      ),
    );
  }
}

class HttpStatusException implements Exception {
  final int statusCode;

  HttpStatusException(this.statusCode);

  @override
  String toString() {
    return 'Error Code: $statusCode';
  }
}
