Demonstration video: https://www.youtube.com/watch?v=HOeuLAFi5nE

The shader applies a wave distortion effect to the sprite particle produced as well as a gradient color with the initial colors provided. Within its script, the shader's wave intensity and gradient colors cycle over time. 

The chain system has joints with a bias of 0.2 and softness of 0.1, with chain segments having a default mass of 1kgs. This felt the most realistic and "chain-like". Pressing spacebar applies a static force to the last chain segment to demonstrate large impulse motion, while the player applies a small force when colliding with chain segments to simulate player collision interaction.

The raycast detection continuously draws a visual line that stops when colliding with anything, and specifically triggers the alarm function when that collision is the player. Laser color changes to red, plays an alarm sound effect, and emits smaller particles with shaders when said player is detected.

For player controls, use WASD or arrow keys for movement, and press space to apply a large (static) force to the chain.
