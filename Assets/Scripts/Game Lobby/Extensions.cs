using UnityEngine;

public static class Extensions {
    public static string GetName(this Color color) {
        if (color == Color.black) return "Black";
        if (color == Color.blue) return "Blue";
        if (color == Color.white) return "White";
        if (color == Color.cyan) return "Cyan";
        if (color == Color.magenta) return "Magenta";
        if (color == Color.green) return "Green";
        if (color == Color.red) return "Red";
        if (color == Color.gray) return "Gray";
        if (color == Color.white) return "White";
        if (color == Color.grey) return "Grey";
        return color == Color.yellow ? "Yellow" : "Unknown";
    }
}