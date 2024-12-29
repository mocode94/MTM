import tkinter as tk

class ToggleSwitch:
    def __init__(self, master):
        self.master = master
        self.master.title("Toggle Switch Example")

        # Create a Canvas for the toggle switch
        self.canvas = tk.Canvas(master, width=60, height=30, bg="lightgray", borderwidth=2, relief="raised")
        self.canvas.pack(pady=20)

        # Draw the switch knob
        self.switch_knob = self.canvas.create_oval(2, 2, 35, 35, fill="white", outline="black")
        
        # Set initial state
        self.is_on = False

        # Bind mouse click to toggle the switch
        self.canvas.bind("<Button-1>", self.toggle)

        # Update the toggle state display
        self.label = tk.Label(master, text="Status: Off", font=("Helvetica", 16))
        self.label.pack(pady=10)

    def toggle(self, event):
        self.is_on = not self.is_on  # Toggle state

        # Move the knob based on the toggle state
        if self.is_on:
            self.canvas.move(self.switch_knob, 30, 0)  # Move to the right
            self.label.config(text="Status: On")
            self.canvas.config(bg="lightgreen")
        else:
            self.canvas.move(self.switch_knob, -30, 0)  # Move to the left
            self.label.config(text="Status: Off")
            self.canvas.config(bg="lightgray")


if __name__ == "__main__":
    root = tk.Tk()
    toggle_switch = ToggleSwitch(root)
    root.mainloop()
