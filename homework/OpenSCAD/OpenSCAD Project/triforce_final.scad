//Quintan Meinung
//Triforce Model


//Create the rays of light from the Triforce
//With the use of recursion    
module tri_ray(length, width, depth, levels) 
{
    if (levels == 0) {
    // Base case: Draw a straight line segment
        linear_extrude(height = depth)
            polygon(points = [
                [0, 0],
                [length, width],
                [length, -width]
            ]);
    } 
    else 
    {
    // Recursive case: Divide the ray into two smaller rays
        new_length = length / 2;
        new_width = width / 2;
        new_depth = depth / 2; // Adjust depth to control thickness
        
        tri_ray(new_length, new_width, new_depth, levels - 1);
        color("Cyan")
        translate([new_length, 0, 0])
            rotate([0, 0, 360 / 12]) 
    // Divide the circle into 12 rays
                tri_ray(new_length, new_width, new_depth, levels - 1);
    }
}

// Create the light with 12 rays below and with a cyan color
for (i = [0 : 360 / 12 : 360 - 360 / 12])
    color("Cyan")
    rotate([0, 0, i])
        tri_ray(100, 10, 5, 4);

//Create surronding yellow light with 12 rays around the Triforce 
for (i = [0 : 360 / 12 : 360 - 360 / 12])
    color("Yellow")
    rotate([10, -20, i])
        tri_ray(100, 5, 5, 7);

//Create a stand with a cube and cylinder
//Move it below the Triforce
//Give it a black and white color
union()
{
    translate([-10,-10,0])
    color("black") cube([30,30,5]);
    translate([5,5,5])
    color("white") cylinder(d=50, h=5);
}

//Create the Three Triangles 
//With points into polygons
//And rotate them facing 90 degrees
//And translate them above the stand
rotate([90,0,0])
    color("Gold")
        translate([5,10,-5])
        {
            points1 =
            [
                [0,0],
                [20, 0],
                [10, 20]
            ];

            points2 =
            [
                [-10, 20],
                [10, 20],
                [0, 40]
            ];

            points3 =
            [
                [-10, 20],
                [0, 0],
                [-20, 0]
            ];

            polygon(points1);
            polygon(points2);
            polygon(points3);
        }