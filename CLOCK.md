package tutorial;
// from luuhoang
import java.applet.Applet;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.TimeZone;
import java.util.Timer;
import java.util.TimerTask;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JEditorPane;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.JTextPane;
import javax.swing.border.Border;
import javax.swing.border.EmptyBorder;

public class test extends Applet{

	GregorianCalendar cal;
    Timer clockTimer = new Timer();
    TimeZone clockTimeZone = TimeZone.getTimeZone("Asia/Ho_Chi_Minh");
    int fs = 30;
	private static final String ADD_DATA = "INSERT INTO Dongho (Dongho_Name, Dongho_Time, Dongho_Duration) VALUES (?, ?, ?)";

	private static final String DELETE_DATA = "DELETE FROM Dongho WHERE Dongho_Name = ?";

	private static final String SELECT_DATA = "SELECT * FROM Dongho";
	
	private static final String SEARCH_DATA = "SELECT * FROM Dongho WHERE Dongho_Name like ?";

	private static JTextArea jTextArea;
	
	private static JTextArea  congviec;

	private static JTextField jTextFieldKey;
	
	private static int leg = 250;
	
	public test(){
		clockTimer.schedule(new TickTimerTask(), 0, 1000);
	}
	
	public static void main(String[] args) {
		MyFrame();
	}

	public static void MyFrame() {
		
		JFrame frame = new JFrame();
		frame.setSize(1200, 700);
		frame.setTitle("Dong Ho");
		
		JPanel panelLeft = new JPanel();
		JPanel panelRight = new JPanel();

		JPanel jPanel1 = new JPanel();
		JLabel jLabelData = new JLabel("Import data");
		JTextField jTextFieldData = new JTextField();
		jTextFieldData.setPreferredSize(new Dimension(150, 30));
		JButton jButtonData = new JButton("Import file");
		jButtonData.addActionListener((e) -> {
			List<String> list = readFile(jTextFieldData.getText());
			for (String string : list) {
				String[] strings = string.split(", ");
				System.out.println(strings[0]+strings[1]+strings[2]);
				String name = strings[0];
				String time = strings[1].replace('h', ':');
				int duration = Integer.parseInt(strings[2]);
				add_data(name, time, duration);
			}
		});

		jPanel1.add(jLabelData);
		jPanel1.add(jTextFieldData);
		jPanel1.add(jButtonData);
		//jPanel1.setBackground(Color.blue);

		//Border border = BorderFactory.createLineBorder(Color.white, 8);
		JPanel jPanel2 = new JPanel();
		JLabel jLabelKey = new JLabel("Keyword    ");
		//JLabel jLabelSpace = new JLabel("");
		//jLabelKey.setBorder(border);
		jTextFieldKey = new JTextField();
		jTextFieldKey.setPreferredSize(new Dimension(248, 30));
		jPanel2.add(jLabelKey);
		//jPanel2.add(jLabelSpace);
		jPanel2.add(jTextFieldKey);
		//jPanel2.setBackground(Color.red);

		JPanel jPanel3 = new JPanel();
		jPanel3.setLayout(new FlowLayout(0,20,10));
		JButton jButton1 = new JButton("Delete");
		jButton1.addActionListener((e) -> {
			delete_data(jTextFieldKey.getText());
		});
		
		JButton jButton2 = new JButton("Search");
		jButton2.addActionListener((e) -> {
			jTextArea.setText(searchName(jTextFieldKey.getText()));
		});
		JButton jButton3 = new JButton("Show all");
		jButton3.addActionListener((e) -> {
			jTextArea.setText(getAll());
		});
		jPanel3.add(jButton1);
		jPanel3.add(jButton2);
		jPanel3.add(jButton3);

		JPanel jPanel4 = new JPanel();
		jTextArea = new JTextArea(20,30);
		jTextArea.setFont(new Font("Arial",Font.PLAIN,20));
		jTextArea.setEditable(false);
		JScrollPane scrollPane = new JScrollPane(jTextArea,
                JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED,
                JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
		scrollPane.setBounds(0, 0, 600, 50);
		panelRight.add(jPanel1);
		panelRight.add(jPanel2);
		panelRight.add(jPanel3);
		panelRight.add(scrollPane);
		GridLayout gridLayout = new GridLayout();
		frame.setLayout(gridLayout);
		
		
		// clock
		Border border =	BorderFactory.createLineBorder(Color.black, 5);
        test clock2d = new test();
        clock2d.setPreferredSize(new Dimension(540,540));
        clock2d.init();
        
        congviec = new JTextArea(3,12);
        congviec.setFont(new Font("Arial",Font.PLAIN,20));
        congviec.setEditable(false);
        JScrollPane spEditor = new JScrollPane(congviec,
                JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED,
                JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        spEditor.setBounds(0, 0, 400, 50);
        panelLeft.setLayout(new FlowLayout(FlowLayout.RIGHT));
        panelLeft.add(spEditor);
        panelLeft.add(clock2d);
        
        panelLeft.setBorder(border);
        JPanel pale = new JPanel();
        
        JPanel pale1 = new JPanel();
        JPanel pale2 = new JPanel();
        JPanel pale3 = new JPanel();
        JTextArea a = new JTextArea("aaa\naaa");
        JTextArea b = new JTextArea("aaa\naaa");
        JTextArea c = new JTextArea("aaa\naaa");
        a.setVisible(false);
        b.setVisible(false);
        c.setVisible(false);
        pale1.add(a);
        pale2.add(b);
        pale.setLayout(new BorderLayout());
        pale.add(pale1, BorderLayout.NORTH);
        pale.add(pale2, BorderLayout.SOUTH);
        pale.add(pale3, BorderLayout.WEST);
        pale.add(panelLeft, BorderLayout.CENTER);
        
		//panelLeft.setBackground(Color.red);
		//panelLeft.setBackground(Color.red);

		frame.add(pale);
		frame.add(panelRight);

		frame.setLocationRelativeTo(null);
		frame.setDefaultCloseOperation(3);
		frame.setVisible(true);
	}
	
	public void init() {}
	
	public void paint(Graphics g) {
  	  	g.setColor(Color.green);
        g.fillOval(45, 45, 410,410);
        g.setColor(Color.WHITE);
        g.fillOval(50, 50, 400, 400);
        double second = cal.get(Calendar.SECOND);
        double minute = cal.get(Calendar.MINUTE);
        double hours = cal.get(Calendar.HOUR);
        //double milis = cal.get(Calendar.MILLISECOND);
        //Vẽ mặt đồng hồ lh
        g.setFont(new Font("Arial",Font.BOLD,fs));
        FontMetrics fm = g.getFontMetrics();
        for (int i = 0; i < 60; i++) {
            int length = 90;
            double rad = (i * 6) * (Math.PI) / 180;
            int x = leg + (int)(220 * Math.cos(rad - (Math.PI / 2)));
            int y = leg + (int)(220 * Math.sin(rad - (Math.PI / 2)));
            String str = i/5+"";
            if (i==0) str = "12";
            int w = fm.stringWidth(str);
            int h = fm.getHeight();
            g.setColor(Color.black);
            if (i % 15 == 0)g.drawString(str, x-w/2, y+h/3);
        }
        //vẽ kim đồng hồ
        drawHands(g, second, minute, hours);
    }
	
	public void drawHands(Graphics g, double second, double minute, double hours) {
  	  
        double rSecond = (second * 6) * (Math.PI) / 180;
        double rrSecond = (second * 6 + 10) * (Math.PI) / 180;
        double rrrSecond = (second * 6 - 10) * (Math.PI) / 180;
        double rMinute = ((minute + (second / 60)) * 6) * (Math.PI) / 180;
        double rrMinute = ((minute + (second / 60)) * 6 + 10) * (Math.PI) / 180;
        double rrrMinute = ((minute + (second / 60)) * 6 -10) * (Math.PI) / 180;
        double rHours = ((hours + (minute / 60)) * 30) * (Math.PI) / 180;
        double rrHours = ((hours + (minute / 60)) * 30 + 20) * (Math.PI) / 180;
        double rrrHours = ((hours + (minute / 60)) * 30 - 20) * (Math.PI) / 180;
        int LKim = 200;
        int LKim1 = 100;
        int[] xPoints = {leg, leg + (int)(LKim/2 * Math.cos(rrMinute - (Math.PI / 2))), leg + (int)(LKim * Math.cos(rMinute - (Math.PI / 2))), leg + (int)(LKim/2 * Math.cos(rrrMinute - (Math.PI / 2)))}; // X-coordinates
        int[] yPoints = {leg, leg + (int)(LKim/2 * Math.sin(rrMinute - (Math.PI / 2))), leg + (int)(LKim * Math.sin(rMinute - (Math.PI / 2))), leg + (int)(LKim/2 * Math.sin(rrrMinute - (Math.PI / 2)))};
        int[] xxPoints = {leg, leg + (int)(LKim1/2 * Math.cos(rrHours - (Math.PI / 2))), leg + (int)(LKim1 * Math.cos(rHours - (Math.PI / 2))), leg + (int)(LKim1/2 * Math.cos(rrrHours - (Math.PI / 2)))}; // X-coordinates
        int[] yyPoints = {leg, leg + (int)(LKim1/2 * Math.sin(rrHours - (Math.PI / 2))), leg + (int)(LKim1 * Math.sin(rHours - (Math.PI / 2))), leg + (int)(LKim1/2 * Math.sin(rrrHours - (Math.PI / 2)))};
        g.setColor(Color.red);
        g.fillPolygon(xPoints, yPoints, 4);
        g.setColor(Color.blue);
        g.fillPolygon(xxPoints, yyPoints, 4);
        //g.drawLine(150, 150, 150 + (int)(100 * Math.cos(rSecond - (Math.PI / 2))), 150 + (int)(100 * Math.sin(rSecond - (Math.PI / 2))));
        //g.drawLine(150, 150, 150 + (int)(70 * Math.cos(rMinute - (Math.PI / 2))), 150 + (int)(70 * Math.sin((rMinute - (Math.PI / 2)))));
        //g.drawLine(150, 150, 150 + (int)(50 * Math.cos(rHours - (Math.PI / 2))), 150 + (int)(50 * Math.sin(rHours - (Math.PI / 2))));
        //g.drawLine(150, 150, 150+(int)(100*Math.sin(2*Math.PI)),150+(int)(100*Math.cos(2*Math.PI)));
    }
	
	public class TickTimerTask extends TimerTask {

        @Override
        public void run() {
            //throw new UnsupportedOperationException("Not supported yet.");
            cal = (GregorianCalendar) GregorianCalendar.getInstance(clockTimeZone);
            double second = cal.get(Calendar.SECOND);
            double minute = cal.get(Calendar.MINUTE);
            double hours = cal.get(Calendar.HOUR);
            
            congviec.setText(showAll((int)hours,(int)minute));
            
            repaint();
        }
    }
	
	public static Connection getConnection() {
		Connection conn = null;
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			conn = DriverManager.getConnection("jdbc:sqlserver://DESKTOP-K9AL2JT\\LUUHOANG:1433;databaseName=clock;user=sa;password=banhmitamda;Encrypt=False;TrustServerCertificate=True;");
		} catch (Exception ex) {
			ex.printStackTrace();
		}
		return conn;
	}

	public static List<String> readFile(String s) {
		Path path = Paths.get(s);
		List<String> lines = new ArrayList<>();
		try {
			lines = Files.readAllLines(path);
			// lines.forEach(System.out::println);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return lines;
	}

	public static void add_data(String name, String time, int duration) {
		try {
			PreparedStatement pst = getConnection().prepareStatement(ADD_DATA);
			pst.setString(1, name);
			pst.setString(2, time);
			pst.setInt(3, duration);

			int affectedRows = pst.executeUpdate();
			// System.out.println("LOG info >> " + affectedRows + " rows affected after
			// save(ItemGroup)");

		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	public static void delete_data(String name) {
		try {
			PreparedStatement pst = getConnection().prepareStatement(DELETE_DATA);
			pst.setString(1, name);

			int affectedRows = pst.executeUpdate();
			System.out.println("LOG info >> " + affectedRows + " rows affected after save(ItemGroup)");

		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	public static String getAll() {
		String s = "----------------Get All---------------\n";
		int cnt = 1;
		try {
			Statement st = getConnection().createStatement();
			ResultSet rs = st.executeQuery(SELECT_DATA);
			while (rs.next()) {
				String name = rs.getString("Dongho_Name");
				String time = rs.getString("Dongho_Time");
				int duration = rs.getInt("Dongho_Duration");
				s += cnt+". "+ name+ ", " + time+", " + duration + "\n";
				cnt++;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}

		return s;
	}
	
	public static String showAll(int hours, int minute) {
		String s = "";
		try {
			
			PreparedStatement ps = getConnection().prepareStatement(SELECT_DATA);
			ResultSet rs = ps.executeQuery();
			while (rs.next()) {
				String name = rs.getString("Dongho_Name");
				String time = rs.getString("Dongho_Time");
				int duration = rs.getInt("Dongho_Duration");
				
				
				String[] strings = time.split(":");
				
				int hours1 = Integer.parseInt(strings[0]);
				int minute1 = Integer.parseInt(strings[1]);
				int hours2 = Integer.parseInt(strings[0]) + duration/60;
				int minute2= Integer.parseInt(strings[1])+ duration%60;
				if(minute2 >=60) {
					minute2= minute2 %60;
					hours2 = hours2+1;
				}
				s+=name+'\n';
				//System.out.println(hours+":"+minute+" "+hours1+":"+minute1+" "+hours2+":"+minute2+'\n');
				if(hours > hours1 && hours < hours2) {
					s += name+"\n";
					//System.out.println(name);
				}
				else if(hours >= hours1 && minute >= minute1 && hours < hours2) {
					s += name+"\n";
					//System.out.println(name);
				}
				else if(hours > hours1 && hours <= hours2 && minute <= minute2) {
					s += name+"\n";
					//System.out.println(name);
				}
				else if(hours >= hours1 && minute >= minute1 && hours <= hours2 && minute <= minute2) {
					s += name+"\n";
					//System.out.println(name);
				}
				
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		if (s != null && s.length() > 0) {
			s = s.substring(0, s.length() - 1);
			}
		return s;
	}
	
	public static String searchName(String name) {
		String s = "Search result:\n";
		try {
			PreparedStatement ps = getConnection().prepareStatement(SEARCH_DATA);
			ps.setString(1, "%" + name + "%");
			ResultSet rs = ps.executeQuery();
			while (rs.next()) {
				String namee = rs.getString("Dongho_Name");
				String time = rs.getString("Dongho_Time");
				s += namee + ", " + time + "\n";
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return s;
	}
	
Day di hoc buoi sang, 06h30, 1000
Day di hoc buoi trua, 12h00, 100
Hoc Toan, 08h00, 500
Hoc Van, 10h00, 300
}
