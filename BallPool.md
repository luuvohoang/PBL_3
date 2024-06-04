package tutorial;

import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.util.Random;
import javax.swing.*;

public class label extends JFrame {
    static int w = 600, h = 400;
    static int of = 50;
    static Ball balls[];
    Random rand = new Random();
    BufferedImage img;
    Graphics g;
    Ball draggedBall = null;
    Point lastMousePosition = null;

    public static void main(String[] args) {
        new label();
    }

    public label() {
        this.setTitle("Random Balls");
        this.setSize(w + of * 2, h + of * 2);
        this.setDefaultCloseOperation(3);
        balls = new Ball[16];
        for (int i = 0; i < balls.length; i++) {
            balls[i] = new Ball(30, rand.nextInt(w - 60) + 30, rand.nextInt(h - 60) + 30, rand.nextDouble() * 2 - 1,
                    rand.nextDouble() * 2 - 1);
            balls[i].c = new Color(rand.nextInt(256), rand.nextInt(256), rand.nextInt(256));
            balls[i].r =  20;
        }
        for (int i = 0; i < balls.length; i++)
            balls[i].start();

        img = new BufferedImage(w + of * 2, h + of * 2, BufferedImage.TYPE_4BYTE_ABGR);
        g = img.getGraphics();

        this.addMouseListener(new MouseAdapter() {
            @Override
            public void mousePressed(MouseEvent e) {
            	super.mousePressed(e);
                Point mousePoint = e.getPoint();
                for (Ball ball : balls) {
                    if (ball.contains(mousePoint)) {
                        draggedBall = ball;
                        lastMousePosition = mousePoint;
                        break;
                    }
                }
            }

            @Override
            public void mouseReleased(MouseEvent e) {
            	int maxSpeed = 30;
                if (draggedBall != null) {
                    if (lastMousePosition != null) {
                        double dx = e.getX() - lastMousePosition.getX();
                        double dy = e.getY() - lastMousePosition.getY();
                        dx*=-1;dy*=-1;
                        double distance = Math.sqrt(dx * dx + dy * dy);
                        double factor = Math.min(0.05, distance / 100); // Adjust for desired sensitivity
                        System.out.println(factor+'\n');
                        draggedBall.vx += Math.min(dx * factor,maxSpeed);
                        draggedBall.vy += Math.min(dy * factor,maxSpeed);
                        draggedBall.shoot(e.getPoint()); // invisible cueStick
                        double v = Math.sqrt(draggedBall.vx * draggedBall.vx + draggedBall.vy * draggedBall.vy);
                        System.out.println(v+'\n');
                    }
                    draggedBall = null;
                    lastMousePosition = null;
                }
            }
        });

        this.addMouseMotionListener(new MouseMotionAdapter() {
            @Override
            public void mouseDragged(MouseEvent e) {
            	super.mouseDragged(e);
                if (draggedBall != null) {
                    Point mousePoint = e.getPoint();
                    draggedBall.setCueStickPosition(mousePoint);
                    repaint();
                }
            }
        });

        this.setVisible(true);
    }

    public void paint(Graphics g1) {
        g.setColor(Color.WHITE);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        g.setColor(Color.BLUE);
        g.drawRect(of, of, w, h);

        for (Ball b : balls) {
            b.draw(g, of);
        }
        g1.drawImage(img, 0, 0, null);
        this.repaint();
    }
}

class Ball extends Thread {
    int r;
    double x, y, vx, vy;
    Color c = Color.YELLOW;
    double k = 0.1;
    Point cueStick;

    public Ball(int r, double x, double y, double vx, double vy) {
        this.r = r;
        this.x = x;
        this.y = y;
        this.vx = vx;
        this.vy = vy;
        this.cueStick = null; // Initialize cue stick as null
    }

    public void run() {
        while (true) {
        	while (true) {
                x = x + vx;
                y = y + vy;

                if (vx < 0 && x <= r)
                    vx = -vx;

                if (vx > 0 && x + r >= label.w)
                    vx = -vx;

                if (vy < 0 && y <= r)
                    vy = -vy;

                if (vy > 0 && y + r >= label.h)
                    vy = -vy;

                MyVector v = new MyVector(vx, vy);
                if (v.Norm()>=k) {
                    v = v.Sub(v.toInit().Mult(k));
                } else {
                    v = new MyVector(0, 0);
                }
                vx = v.x;
                vy = v.y;

                for (Ball b:label.balls) {
                    if (this==b) continue;
                    MyVector p1 = new MyVector(this.x, this.y);
                    MyVector p2 = new MyVector(b.x, b.y);
                    MyVector v1 = new MyVector(this.vx, this.vy);
                    MyVector v2 = new MyVector(b.vx, b.vy);
                    MyVector p12 = p2.Sub(p1);
                    if (p12.Norm()<=this.r+b.r) {
                        MyVector e = p12.toInit();
                        MyVector v11 = e.Mult(v1.DotP(e));
                        MyVector v12 = v1.Sub(v11);

                        MyVector v21 = e.Mult(v2.DotP(e));
                        MyVector v22 = v2.Sub(v21);

                        if (v11.DotP(e)>=v12.DotP(e)) {
                            v1 = v21.Add(v12);
                            this.vx = v1.x;
                            this.vy = v1.y;
                            v2 = v11.Add(v22);
                            b.vx = v2.x;
                            b.vy = v2.y;
                        }
                    }
                }

                try {
                    Thread.sleep(10);
                } catch (InterruptedException e) {
                }
            }
        }
    }

    public void draw(Graphics g, int offset) {
        g.setColor(c);
        g.fillOval(offset + (int) x - r, offset + (int) y - r, r * 2, r * 2);
        g.setColor(Color.BLACK);
        g.drawOval(offset + (int) x - r, offset + (int) y - r, r * 2, r * 2);

        if (cueStick != null) {
            g.setColor(Color.RED);
            g.drawLine(offset + (int) x, offset + (int) y, cueStick.x, cueStick.y);
        }
    }

    public boolean contains(Point point) {
        return Math.sqrt(Math.pow(point.x - (x + label.of), 2) + Math.pow(point.y - (y + label.of), 2)) <= r;
    }

    public void setCueStickPosition(Point point) {
        this.cueStick = point;
    }

    public void shoot(Point releasePoint) {
        // Calculate velocity based on cue stick position
//        vx = (releasePoint.x - cueStick.x) * 0.01; // Adjust as needed
//        vy = (releasePoint.y - cueStick.y) * 0.01; // Adjust as needed
        this.cueStick = null; // Cue stick disappears after shooting
    }
}
class MyVector{
    double x,y;
    public MyVector(double x, double y) {
        this.x = x;
        this.y = y;
    }
    MyVector Add(MyVector a) {
        return new MyVector(x+a.x, y+a.y);
    }
    MyVector Sub(MyVector a) {
        return new MyVector(x-a.x, y-a.y);
    }
    MyVector Mult(double a) {
        return new MyVector(x*a, y*a);
    }
    double DotP(MyVector a) {
        return x*a.x+y*a.y;
    }
    double Norm() {
        return Math.sqrt(x*x+y*y);
    }
    MyVector toInit() {
        return this.Mult(1.0/this.Norm());
    }
}
