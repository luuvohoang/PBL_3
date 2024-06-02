package tutorial;
// from luuhoang
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.GridLayout;
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

public class test1{

	private static final String ADD_DATA = "INSERT INTO ICPC (TeamName, UniversityName, ProblemID, Times, Result) VALUES (?, ?, ?, ?, ?)";

	private static final String DELETE_DATA = "DELETE FROM ICPC WHERE TeamName = ?";
	
	private static final String RANKING_DATA = "SELECT \r\n"
			+ "    *,\r\n"
			+ "    RANK() OVER (ORDER BY pro DESC, Total) AS Ranks\r\n"
			+ "	FROM(\r\n"
			+ "SELECT * FROM (SELECT TeamName,UniversityName,COUNT(ProblemID) as pro, SUM(Timess) AS [Total]\r\n"
			+ "FROM ( SELECT distinct TeamName,UniversityName,ProblemID, min(Times) as Timess\r\n"
			+ "					FROM ICPC\r\n"
			+ "					where Result = 'AC'  \r\n"
			+ "					GROUP BY TeamName,UniversityName,ProblemID) as tablee\r\n"
			+ "GROUP BY TeamName,UniversityName) as ranking) as ranking1;";

	private static final String SELECT_DATA = "select *  \r\n"
			+ "from (select *, RANK() OVER (PARTITION BY UniversityName ORDER BY pro DESC, Total) AS Rankss \r\n"
			+ "	from (SELECT *,RANK() OVER (ORDER BY pro DESC, Total) AS Ranks\r\n"
			+ "		FROM(SELECT * \r\n"
			+ "			FROM (SELECT TeamName,UniversityName,COUNT(ProblemID) as pro, SUM(Timess) AS [Total]\r\n"
			+ "				FROM ( SELECT distinct TeamName,UniversityName,ProblemID, min(Times) as Timess\r\n"
			+ "							FROM ICPC\r\n"
			+ "							where Result = 'AC'  \r\n"
			+ "							GROUP BY TeamName,UniversityName,ProblemID\r\n"
			+ "					) as tablee\r\n"
			+ "				GROUP BY TeamName,UniversityName\r\n"
			+ "				) as ranking\r\n"
			+ "			) as ranking1\r\n"
			+ "		) as ranking2\r\n"
			+ "	) as ranking3 where Rankss = 1;";
	
	private static final String SEARCH_DATA = "select * from (SELECT \r\n"
			+ "    *,\r\n"
			+ "    RANK() OVER (ORDER BY pro DESC, Total) AS Ranks\r\n"
			+ "	FROM(\r\n"
			+ "SELECT * FROM (SELECT TeamName,UniversityName,COUNT(ProblemID) as pro, SUM(Timess) AS [Total]\r\n"
			+ "FROM ( SELECT distinct TeamName,UniversityName,ProblemID, min(Times) as Timess\r\n"
			+ "					FROM ICPC\r\n"
			+ "					where Result = 'AC'  \r\n"
			+ "					GROUP BY TeamName,UniversityName,ProblemID) as tablee\r\n"
			+ "GROUP BY TeamName,UniversityName) as ranking) as ranking1) as ranking2 WHERE TeamName like ?";

	private static JTextArea jTextArea;
	
	private static JTextArea  congviec;

	private static JTextField jTextFieldKey;
	
	
	public test1(){
		
	}
	
	public static void main(String[] args) {
		MyFrame();
	}

	public static void MyFrame() {
		
		JFrame frame = new JFrame();
		frame.setSize(500, 600);
		frame.setTitle("Quan ly ket qua ICPC");
		
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
				String TeamName = strings[0];
				String UniversityName = strings[1];
				String ProblemID = strings[2];
				int Times = Integer.parseInt(strings[3]);
				String Result = strings[4];
				add_data(TeamName, UniversityName, ProblemID, Times, Result);
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
		jTextFieldKey.setPreferredSize(new Dimension(200, 30));
		jPanel2.add(jLabelKey);
		//jPanel2.add(jLabelSpace);
		jPanel2.add(jTextFieldKey);
		//jPanel2.setBackground(Color.red);

		JPanel jPanel3 = new JPanel();
		jPanel3.setLayout(new FlowLayout(0,20,10));
		JButton jButton1 = new JButton("Ranking");
		jButton1.addActionListener((e) -> {
			jTextArea.setText(ranking_data());
		});
		
		JButton jButton2 = new JButton("Search");
		jButton2.addActionListener((e) -> {
			jTextArea.setText(searchName(jTextFieldKey.getText()));
		});
		JButton jButton3 = new JButton("Won Team");
		jButton3.addActionListener((e) -> {
			jTextArea.setText(getAll());
		});
		jPanel3.add(jButton1);
		jPanel3.add(jButton2);
		jPanel3.add(jButton3);

		JPanel jPanel4 = new JPanel();
		jTextArea = new JTextArea(10,25);
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
		
		
		frame.add(panelRight);

		frame.setLocationRelativeTo(null);
		frame.setDefaultCloseOperation(3);
		frame.setVisible(true);
	}
	
	public void init() {}
	
	public static Connection getConnection() {
		Connection conn = null;
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			conn = DriverManager.getConnection("jdbc:sqlserver://DESKTOP-K9AL2JT\\LUUHOANG:1433;databaseName=icpc;user=sa;password=banhmitamda;Encrypt=False;TrustServerCertificate=True;");
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

	public static void add_data(String TeamName,String UniversityName,String ProblemID,int Times,String Result) {
		try {
			
			PreparedStatement pst = getConnection().prepareStatement(ADD_DATA);
			pst.setString(1, TeamName);
			pst.setString(2, UniversityName);
			pst.setString(3,ProblemID);
			pst.setInt(4, Times);
			pst.setString(5, Result);
			

			int affectedRows = pst.executeUpdate();
			// System.out.println("LOG info >> " + affectedRows + " rows affected after
			// save(ItemGroup)");

		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	public static String ranking_data() {
		String s = "";
		int cnt = 1;
		try {
			Statement st = getConnection().createStatement();
			ResultSet rs = st.executeQuery(RANKING_DATA);
			while (rs.next()) {

				String TeamName = rs.getString("TeamName");
				String UniversityName = rs.getString("UniversityName");
				int pro = rs.getInt("pro");
				int Total = rs.getInt("Total");
				int Ranks = rs.getInt("Ranks");
				s += Ranks+", "+ TeamName+ ", "+ UniversityName+ ", "+ pro+ ", " + Total + "\n";
				cnt++;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}

		return s;
	}

	public static String getAll() {
		String s = "";
		int cnt = 1;
		try {
			Statement st = getConnection().createStatement();
			ResultSet rs = st.executeQuery(SELECT_DATA);
			while (rs.next()) {
				String TeamName = rs.getString("TeamName");
				String UniversityName = rs.getString("UniversityName");
				int pro = rs.getInt("pro");
				int Total = rs.getInt("Total");
				int Ranks = rs.getInt("Ranks");
				s += Ranks+", "+ TeamName+ ", "+ UniversityName+ ", "+ pro+ ", " + Total + "\n";
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
				String TeamName = rs.getString("TeamName");
				String UniversityName = rs.getString("UniversityName");
				int pro = rs.getInt("pro");
				int Total = rs.getInt("Total");
				String Ranks = rs.getString("Ranks");
				s += Ranks + ", "+ TeamName+ ", "+ UniversityName+ ", "+ pro+ ", " + Total + "\n";
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return s;
	}
	

}

BKDN1, Dai hoc bach khoa - DHDN, ProblemA, 2, AC 
UET, Dai hoc Cong nghe Ha Noi, ProblemB, 4, WA
BKDN2, Dai hoc bach khoa - DHDN, ProblemA, 5, WA
HCMUS, Dai hoc Tu nhien - DHQGHCM, ProblemB, 6, AC
UET, Dai hoc Cong nghe Ha Noi, ProblemB, 7, AC
BKDN2, Dai hoc bach khoa - DHDN, ProblemA, 8, WA
HCMUS, Dai hoc Tu nhien - DHQGHCM, ProblemB, 10, AC
UET, Dai hoc Cong nghe Ha Noi, ProblemA, 10, AC
BKDN1, Dai hoc bach khoa - DHDN, ProblemB, 15, AC
BKDN2, Dai hoc bach khoa - DHDN, ProblemA, 16, AC
