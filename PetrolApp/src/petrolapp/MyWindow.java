/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package petrolapp;

/**
 *
 * @author User
 */
import javax.swing.*;
import java.awt.*;  
import java.awt.event.*;
public class MyWindow extends JFrame implements ActionListener
{
    private String [] years = { "2017", "2018", "2019",};
    private String [] city = { "Johannesburg", "Durban", "Cape Town",};
    private String [][] cost = {{ "R10.72", "R10.35", "R10.20",},{"R12.75","R12.32", "R12.22",},{"R13.70", "R13.31", "R13.23"},};
    
    private JMenuBar mainBar = new JMenuBar();
    private JMenu menu1 = new JMenu("File");
    private JMenu menu2 = new JMenu("Tools");
    private JMenuItem exit = new JMenuItem("Exit");
    private JMenuItem submit = new JMenuItem("Submit");
    private JMenuItem report = new JMenuItem("Report");
    private String Message ;
    private JComboBox comboBox = new JComboBox(years);    
    private JComboBox comboBo = new JComboBox(city);;
    final JList list = new JList(comboBo.getModel());
    private JButton button = new JButton("submit");
    private JPanel panel1 = new JPanel(new FlowLayout());
        
        
    
    
    
    public MyWindow()
    {
        setTitle("Petrol App");
        setSize (350,300);
        setLayout(new FlowLayout());
        JMenuFrame();
        add(new JLabel("City :"));
        add(comboBo);
        comboBo.addActionListener(this);
        add(new JLabel("Year :"));
        add(comboBox);
        comboBox.addActionListener(this);
        add(panel1);
        add(button);
        button.addActionListener(this);
        
        
        setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE );
        setVisible(true);
        
        
        
    }
    
    public void JMenuFrame()
    {
        setLayout(new FlowLayout());
        setJMenuBar(mainBar);
        mainBar.add(menu1);
        mainBar.add(menu2);
        menu1.add(exit);
        menu2.add(submit);
        menu2.add(report);
        exit.addActionListener(this);
        report.addActionListener(this);
        submit.addActionListener(this);
    }
    
    
    
    
    
    @Override
    public void actionPerformed(ActionEvent e)
    {
        Object source = e.getSource();
        Container con = getContentPane();
        int index = comboBox.getSelectedIndex();
        int inde = comboBo.getSelectedIndex();
        if(source == submit)
            for (int x = 0; x <3; x++) 
            {
                for (int y = 0; y <3; y++)
                {
                    if(index==x &&inde==y)
                        JOptionPane.showMessageDialog(null,"city: "+city[y]+"\nYear:"+years[x]+"\nCost:"+cost[y][x]);
                        
                }
            }
                
        if(source == button)
            for (int x = 0; x <3; x++) 
            {
                for (int y = 0; y <3; y++)
                {
                    if(index==x &&inde==y)
                        JOptionPane.showMessageDialog(null,"City: "+city[y]+"\nYear: "+years[x]+"\nCost: "+cost[y][x]);
                        
                }
            }        
        if(source == exit)
            System.exit(0);
        else if(source == report)
            JOptionPane.showMessageDialog(null,"Average Petrol Cost for Johannesburg is R 10.42\nAverage Petrol Cost for Durban is R 12.23\nAverage Petrol Cost for Cape Town is R 13.41\n");
            
    }
        
 }
